using auth_service.authservice.application.dtos;

namespace auth_service.authservice.application.InterfaceApplication
{
    public interface IAuthenticationToken
    {
        public TokenResult GenerateToken(string email, string role, string type);

        public string GetTypeTokenJWT(HttpContext httpContext);

    }
}
