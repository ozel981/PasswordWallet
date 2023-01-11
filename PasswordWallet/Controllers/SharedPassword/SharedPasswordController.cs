using Microsoft.AspNetCore.Mvc;
using PasswordWallet.Authentication;
using PasswordWallet.Models.Password.Commands;
using PasswordWallet.Models.Password.Queries;
using PasswordWallet.Models.SharedPassword.Commands;
using PasswordWallet.Models.User.Commands;
using PasswordWallet.Services.Interfaces;
using PasswordWallet.Services.Password;
using PasswordWallet.Services.User;

namespace PasswordWallet.Controllers.SharedPassword
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SharedPasswordController : BaseController
    {
        private readonly ISharedPasswordService _sharedPasswordService;
        public SharedPasswordController(ISharedPasswordService sharedPasswordService)
        {
            _sharedPasswordService = sharedPasswordService;
        }
        [HttpGet]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _sharedPasswordService
                    .GetSharedPasswords(
                        GetHttpContextAuthData, 
                        new Models.SharedPassword.Queries.GetSharedPasswordsQuery() 
                        { 
                            UserId = GetHttpContextAuthData.UserId 
                        }));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> GetPasword([FromRoute] int id)
        {
            try
            {
                var result = await _sharedPasswordService
                    .GetDecodedSharedPassword(
                        GetHttpContextAuthData,
                        new Models.SharedPassword.Queries.GetDecodedSharedPasswordQuery() 
                        { 
                            Id = id 
                        });
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> Create([FromBody] SharePasswordCommand command)
        {
            try
            {
                return Ok(await _sharedPasswordService.SharePassword(GetHttpContextAuthData, command));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _sharedPasswordService.DeleteSharedPassword(GetHttpContextAuthData, new DeleteSharedPasswordCommand() { Id = id });
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
