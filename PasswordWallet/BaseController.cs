using Microsoft.AspNetCore.Mvc;
using PasswordWallet.Authentication;

namespace PasswordWallet
{
    public class BaseController : Controller
    {
        protected AuthData GetHttpContextAuthData 
        { 
            get 
            {
                var identity = HttpContext.User.Identities.Where(identity => identity.AuthenticationType == "BasicAuth").Single();
                var claims = identity.Claims.ToList();
                var authData = new AuthData
                {
                    UserId = int.Parse(claims.Single(claim => claim.Type == "id").Value),
                    HashKey = claims.Single(claim => claim.Type == "hashKey").Value
                };
                return authData;
            } 
        }
    }
}
