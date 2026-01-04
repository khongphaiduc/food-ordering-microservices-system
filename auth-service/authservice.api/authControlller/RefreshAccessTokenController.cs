using auth_service.authservice.application.dtos;
using auth_service.authservice.application.InterfaceApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace auth_service.authservice.api.authControlller
{
    [Route("api/auth")]
    [ApiController]
    public class RefreshAccessTokenController : ControllerBase
    {
        private readonly IProvideoAccessToken _iprovideAccessToken;
        private readonly IAuthenticationToken _iAuthenticationToken;

        public RefreshAccessTokenController(IProvideoAccessToken provideoAccessToken, IAuthenticationToken authenticationToken)
        {
            _iprovideAccessToken = provideoAccessToken;
            _iAuthenticationToken = authenticationToken;
        }

        // cấp lại access token
        [HttpPost("accesstoken")]
        [Authorize]
        public async Task<IActionResult> RequestProvideAccessToken([FromBody] RequetsProvideAccessToken request)
        {

            var TypeToken = _iAuthenticationToken.GetTypeTokenJWT(this.HttpContext);  // check loại token xem có đúng là refresh token k 
            if (TypeToken != "RefreshToken") return Unauthorized("Token not valid");

            var tokenResult = await _iprovideAccessToken.ProvideAccessToken(request.UserId);
            return Created("", new TokenResult() { TypeToken = tokenResult.TypeToken, Token = tokenResult.Token, TimeCreate = tokenResult.TimeCreate, TimeExpire = tokenResult.TimeExpire });
        }

    }
}
