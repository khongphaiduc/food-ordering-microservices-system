using auth_services.AuthService.Application.DTOS;
using auth_services.AuthService.Application.Service;
using auth_services.AuthService.Infastructure.DbContextAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace auth_services.AuthService.API.AuthControllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISignUpUser _iUserSignUp;
        private readonly ICheckLogin _iUserLogIn;

        public UsersController(ISignUpUser signUpUser, ICheckLogin checkLogin)
        {
            _iUserSignUp = signUpUser;
            _iUserLogIn = checkLogin;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(RequestCreateNewUser user)
        {
            var result = await _iUserSignUp.Execute(user);

            if (result)
            {
                return Created(" ", new { message = "Create New User Successful" });
            }
            else
            {
                return BadRequest("Fail");
            }

        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(RequestUserLogin user)
        {
            var result = await _iUserLogIn.IsUserLoginAsync(user);

            if (!result)
            {
                return Unauthorized("Login Fail");
            }

            return Ok(result);
        }
    }
}
