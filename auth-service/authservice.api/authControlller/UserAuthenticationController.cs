using auth_service.authservice.api.CustomExceptionSerives;
using auth_service.authservice.application.dtos;
using auth_service.authservice.application.InterfaceApplication;
using Microsoft.AspNetCore.Mvc;


namespace auth_service.authservice.api.authControlller
{
    [Route("api/users")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly ICreateUserHandler _createUerHandler;
        private readonly IUserLogin _iUserLogin;
        private readonly IAuthenticationToken _iAuthen;

        public UserAuthenticationController(ICreateUserHandler createUserHandler, IUserLogin userLogin, IAuthenticationToken authenticationToken)
        {
            _createUerHandler = createUserHandler;
            _iUserLogin = userLogin;
            _iAuthen = authenticationToken;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> AuthenticateUser([FromBody] RequestAccount registerAccount)
        {
            var result = await _createUerHandler.HandleCreateUser(registerAccount);
            if (result.IsSuccessful == false)
            {
                return BadRequest(new { Success = result.IsSuccessful, Message = result.Message });
            }
            return Created("", new { Success = result.IsSuccessful, Message = result.Message });
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] RequestAccount loginAccount)
        {
            var result = await _iUserLogin.LoginHandler(loginAccount);

            if (result)
            {
                var infoToken = _iAuthen.GenerateToken(loginAccount.Email, "Customer");
                return Ok(new { account = loginAccount.Email, tokenInfor = infoToken });
            }
            else
            {
                return Unauthorized(new { status = result, email = loginAccount.Email });
            }

        }

    }
}
