using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PasswordWallet.Database;
using PasswordWallet.Database.Entities;
using PasswordWallet.Models.Error;
using PasswordWallet.Models.Register.Commands;
using PasswordWallet.Services.Interfaces;
using System.Linq;
using System.Text;
using ErrorModel = PasswordWallet.Models.Error.ErrorModel;

namespace PasswordWallet.Authentication
{
    public class AuthenticationFilter : ActionFilterAttribute
    {

        private readonly PasswordWalletDbContext _dbContext;
        private readonly IRegisterService _registerService;
        private readonly IHashService _hashService;

        public AuthenticationFilter(PasswordWalletDbContext dbContext, IHashService hashService, IRegisterService registerService)
        {
            _dbContext = dbContext;
            _hashService = hashService;
            _registerService = registerService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            try
            {
                if (httpContext == null) throw new BadHttpRequestException("No http context");
                var ipAddress = await VerifyAddressIP(httpContext.Connection.RemoteIpAddress?.ToString());
                var authString = GetAuthString(httpContext);
                var logInData = DecodeAuthorizationString(authString);
                var user = await GetUser(logInData.Login);
                if (user == null) throw new ArgumentException("No such a user");
                var isPasswordOk = CheckIfPasswordIsOk(user, logInData.Password);
                var registerCommand = new RaportSigninActionCommand()
                {
                    AddressIP = httpContext.Connection.RemoteIpAddress!.ToString(),
                    Session = httpContext.Session.Id,
                    Computer = "computerkey",
                    IsCorrect = isPasswordOk,
                    UserId = user.Id,
                };
                await _registerService.CreateSignInRegister(registerCommand, ipAddress);
                if (isPasswordOk)
                {
                    var claims = new List<System.Security.Claims.Claim>()
                    {
                        new System.Security.Claims.Claim("id", user.Id.ToString()),
                        new System.Security.Claims.Claim("hashKey", _hashService.GetHashMD5(logInData.Password))
                    };
                    var identity = new System.Security.Claims.ClaimsIdentity(claims, "BasicAuth");
                    context.HttpContext.User.AddIdentity(identity);
                }
                else
                {
                    var lockTime = await _registerService.SetLockTime(registerCommand, ipAddress);
                    if (lockTime.HasValue)
                    {
                        throw new UnauthorizedAccessException($"Wrong password. Too many trials. Sign in locked for a {lockTime.Value} seconds");
                    }
                    else
                    {
                        throw new UnauthorizedAccessException($"Wrong password.");
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                context.Result = GetUnauthorizedObjectResult(new ErrorModel() { FieldName = "Authorization", Message = e.Message });
                return;
            }
            catch (BadHttpRequestException e)
            {
                context.Result = GetBadRequestObjectResult(new ErrorModel() { FieldName = "Authorization", Message = e.Message });
                return;
            }
            catch (ArgumentException e)
            {
                context.Result = GetNotFoundObjectResult(new ErrorModel() { FieldName = "Authorization", Message = e.Message });
                return;
            }
            catch (Exception e)
            {
                context.Result = GetUnauthorizedObjectResult(new ErrorModel() { FieldName = "Authorization", Message = e.Message });
                return;
            }

            await next();
        }

        private string GetAuthString(HttpContext context)
        {
            if (!context.Request.Headers.Authorization.Any()) throw new BadHttpRequestException("No http header");
            var authorizationStrings = context.Request.Headers.Authorization;
            var authorizationString = "";
            try
            {
                authorizationString = authorizationStrings.Single();
            }
            catch
            {
                throw new UnauthorizedAccessException("More than one authorization string");
            }
            if (string.IsNullOrEmpty(authorizationString)) throw new UnauthorizedAccessException("No authorization string");
            return authorizationString;
        }

        private async Task<IpAddress> VerifyAddressIP(string? addressIP)
        {
            if (string.IsNullOrEmpty(addressIP)) throw new BadHttpRequestException("Ip address not found");
            var ipAddress = await _registerService.GetOrCreateIpAddress(addressIP);
            if (ipAddress == null)
            {
                throw new UnauthorizedAccessException($"Wrong ip address");
            }
            if (ipAddress != null && ipAddress.LockDate.HasValue && ipAddress.LockDate.Value > DateTime.Now)
            {
                var diffOfDates = ipAddress.LockDate.Value - DateTime.Now;
                throw new UnauthorizedAccessException($"Signing in is locked for a {diffOfDates.Seconds} seconds");
            }
            return ipAddress;
        }

        private (string Login, string Password) DecodeAuthorizationString(string authorizationString)
        {
            var authStringsSplit = authorizationString.Split(' ');
            var authorizationToken = "";
            if (authStringsSplit.Length != 2 || !string.Equals(authStringsSplit[0], "Basic")) throw new UnauthorizedAccessException("Bad auth string format");
            authorizationToken = authStringsSplit[1];
            var converedToken = Convert.FromBase64String(authorizationToken);
            var decodedToken = Encoding.UTF8.GetString(converedToken);
            var userPassword = decodedToken.Split(':');
            if (userPassword.Length != 2) throw new UnauthorizedAccessException("Bad auth string format");
            return (userPassword[0], userPassword[1]);
        }

        private ErrorResponse GetErrorResponse(params ErrorModel[] errorModels)
        {
            var errorResponse = new ErrorResponse();

            foreach (var errorModel in errorModels)
            {
                errorResponse.Errors.Add(errorModel);
            }

            return errorResponse;
        }

        private IActionResult GetUnauthorizedObjectResult(params ErrorModel[] errorModels)
        {
            return new UnauthorizedObjectResult(GetErrorResponse(errorModels));
        }

        private IActionResult GetBadRequestObjectResult(params ErrorModel[] errorModels)
        {
            return new BadRequestObjectResult(GetErrorResponse(errorModels));
        }

        private IActionResult GetNotFoundObjectResult(params ErrorModel[] errorModels)
        {
            return new NotFoundObjectResult(GetErrorResponse(errorModels));
        }

        private async Task<Database.Entities.User?> GetUser(string login)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => string.Equals(u.Login, login));

            return user;
        }

        private bool CheckIfPasswordIsOk(Database.Entities.User? user, string password)
        {
            if (user != null)
            {
                var hashPassword = _hashService.GetHash(password, user.Salt, user.IsPasswordKeptAsHash);

                return string.Equals(user.Password, hashPassword);
            }
            return false;
        } 
    }
}
