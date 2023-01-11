using Microsoft.AspNetCore.Mvc;
using PasswordWallet.Authentication;
using PasswordWallet.Services.Interfaces;
using PasswordWallet.Models.User.Commands;
using PasswordWallet;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using PasswordWallet.Models.Password.Commands;
using PasswordWallet.Models.Password.Queries;

namespace PasswordWallet.Password
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PasswordController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        public PasswordController(IUserService userService, IPasswordService passwordService)
        {
            _userService = userService;
            _passwordService = passwordService;
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _passwordService.GetUserPasswords(new GetPasswordsQuery() { UserId = GetHttpContextAuthData.UserId }));
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
                var result = await _passwordService.GetDecodedPassword(GetHttpContextAuthData, new GetDecodedPasswordQuery() { Id = id });
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> Create([FromBody] CreatePasswordCommand command)
        {
            try
            {
                return Ok(await _passwordService.CreatePassword(GetHttpContextAuthData, command));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePasswordCommand command)
        {
            try
            {
                command.Id = id;
                await _passwordService.UpdatePassword(GetHttpContextAuthData, command);
                return Ok();
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
                await _passwordService.DeletePassword(GetHttpContextAuthData, new DeletePasswordCommand() { Id = id });
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
