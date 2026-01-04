using auth_service.authservice.application.dtos;
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

        //  tạo jwt
        public TokenResult GenerateToken(string email, string role, string type)
        {
            int time;
            DateTime timeExpire;
            if (type == "AccessToken")
            {
                time = int.Parse(_iconfig["Jwt:Time:AccessToken"]!);
                timeExpire = DateTime.UtcNow.AddMinutes(time);
            }
            else
            {
                time = int.Parse(_iconfig["Jwt:Time:RefreshToken"]!);
                timeExpire = DateTime.UtcNow.AddDays(time);
            }

            var listClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("TokenType",type)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfig["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _iconfig["JWT:Issuer"],
                    audience: _iconfig["JWT:Audience"],
                    claims: listClaim,
                    expires: timeExpire,
                    signingCredentials: creds);
            return new TokenResult { TypeToken = type, Token = new JwtSecurityTokenHandler().WriteToken(token), TimeExpire = timeExpire, TimeCreate = DateTime.Now };
        }

        public string GetTypeTokenJWT(HttpContext httpContext)
        {
            // Lấy token từ header Authorization
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token not found");

            // Tạo handler để đọc token
            var handler = new JwtSecurityTokenHandler();

            // Parse token thành JwtSecurityToken
            var jwtToken = handler.ReadJwtToken(token);

            // Lấy claim TypeToken
            var typeTokenClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "TypeToken")?.Value;

            if (typeTokenClaim == null)
                throw new Exception("TypeToken claim not found");

            return typeTokenClaim;
        }

    }
}
