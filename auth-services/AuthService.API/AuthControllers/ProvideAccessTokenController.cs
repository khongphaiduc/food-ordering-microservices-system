using auth_services.AuthService.Application.DTOS;
using auth_services.AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserService.API.Protos;

namespace auth_services.AuthService.API.AuthControllers
{
    [Authorize(AuthenticationSchemes = "RefreshToken")]
    [Route("api/auth")]
    [ApiController]
    public class ProvideAccessTokenController : ControllerBase
    {
        private readonly IProvideAccessToken _iProvideToken;
        private readonly UserInfoGrpc.UserInfoGrpcClient _iUserClient;

        public ProvideAccessTokenController(IProvideAccessToken provideAccessToken, UserInfoGrpc.UserInfoGrpcClient userInfoGrpcClient)
        {
            _iProvideToken = provideAccessToken;
            _iUserClient = userInfoGrpcClient;
        }

        [HttpPost("accesstoken")]
        public async Task<IActionResult> AccessToken([FromBody] RequestProvideAccessToken request)
        {
            var token = await _iProvideToken.Handle(request);
            if (!token.IsSuccess)
            {
                return BadRequest(new
                {
                    status = false,
                    message = "User not exit"
                });
            }
            return Ok(token);
        }


        [AllowAnonymous]
        [HttpGet("testCreate")]
        public IActionResult Index()
        {
            var s = _iUserClient.CreateNewInformationUser(new CreateNewInformationUserRequest
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sơn Tùng MTP",
                Email="sontungmtp@gmail.com",
                Phone= "0123456789"
            });

            return Ok(s);
        }
    }
}
