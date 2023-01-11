using Microsoft.AspNetCore.Mvc;
using PasswordWallet.Authentication;
using PasswordWallet.Services.Interfaces;
using PasswordWallet;
using System.Linq;
using PasswordWallet.Models.User.Commands;

namespace PasswordWallet.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            return Ok(await _userService.CreateUser(command));
        }

        [HttpPut("changePassword")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> ChangePassword([FromBody] UpdateUserCommand command)
        {
            try
            {
                await _userService.UpdateUser(GetHttpContextAuthData,command);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("updateUser")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            try
            {
                await _userService.UpdateUser(GetHttpContextAuthData,command);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("signIn")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> SignIn()
        {
            try 
            {
                var user = await _userService.GetUser(GetHttpContextAuthData.UserId);
                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
