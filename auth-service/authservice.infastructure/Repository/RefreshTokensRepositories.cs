using auth_service.authservice.application.dtos;
using auth_service.authservice.application.InterfaceApplication;
using auth_service.authservice.infastructure.dbcontexts;
using auth_service.authservice.infastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace auth_service.authservice.infastructure.Repository
{
    public class RefreshTokensRepositories : IRefreshTokensRepositories
    {
        private readonly FoodAuthContext _db;
        private readonly IAuthenticationToken _iauthe;


        public RefreshTokensRepositories(FoodAuthContext foodAuthContext, IAuthenticationToken authenticationToken)
        {
            _db = foodAuthContext;
            _iauthe = authenticationToken;

        }

        public async Task<TokenResult> AddRefreshToken(Guid idUser)
        {
            var user = await _db.Users.Include(s => s.Roles).FirstOrDefaultAsync(s => s.Id == idUser);

            // tạo refresh token
            var refreshToken = _iauthe.GenerateToken(user.Email, user.Roles.FirstOrDefault()?.Name, "RefreshToken");


            _db.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshToken.Token,
                UserId = idUser,
                ExpiresAt = refreshToken.TimeExpire,
                Device = "Web"
            });

            int row = await _db.SaveChangesAsync();

            if (row > 0)
            {
                return new TokenResult
                {
                    TypeToken = refreshToken.TypeToken,
                    Token = refreshToken.Token,
                    TimeCreate = DateTime.Now,
                    TimeExpire = refreshToken.TimeExpire

                };
            }
            else
            {
                return new TokenResult();
            }

        }

        // check xem token đã bị thu hồi hay chưa
        public async Task<bool> IsInvokedToken(string token)
        {
            return await _db.RefreshTokens.AnyAsync(s => s.Token == token && s.RevokedAt != null);
        }

        // thu hoi token cu
        public async Task RevokeOldToken(Guid idUser)
        {
            var userRrefreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(s => s.UserId == idUser && s.RevokedAt == null);

            if (userRrefreshToken != null)
            {
                userRrefreshToken.RevokedAt = DateTime.Now;
                await _db.SaveChangesAsync();
            }

        }

    }
}
