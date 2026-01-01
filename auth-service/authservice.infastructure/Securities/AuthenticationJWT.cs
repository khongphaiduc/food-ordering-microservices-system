using auth_service.authservice.application.InterfaceApplication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth_service.authservice.infastructure.Securities
{
    public class AuthenticationJWT : IAuthenticationToken
    {
        private readonly IConfiguration _iconfig;

        public AuthenticationJWT(IConfiguration configuration)
        {
            _iconfig = configuration;
        }

        public object GenerateToken(string email, string role)
        {

            var time = int.Parse(_iconfig["Jwt:Time"]!);
            var listClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfig["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _iconfig["JWT:Issuer"],
                    audience: _iconfig["JWT:Audience"],
                    claims: listClaim,
                    expires: DateTime.UtcNow.AddHours(time),
                    signingCredentials: creds);
            return new { token = new JwtSecurityTokenHandler().WriteToken(token), timeExpire = time, timeCreate = DateTime.Now };
        }

    }
}
