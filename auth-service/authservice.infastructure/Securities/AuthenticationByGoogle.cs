using auth_service.authservice.application.dtos;
using auth_service.authservice.application.InterfaceApplication;
using auth_service.authservice.infastructure.dbcontexts;
using auth_service.authservice.infastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_service.authservice.infastructure.Securities
{
    public class AuthenticationByGoogle : IAuthenticationByGoogle
    {
        private readonly FoodAuthContext _db;

        public AuthenticationByGoogle(FoodAuthContext foodAuthContext)
        {
            _db = foodAuthContext;
        }

        public async Task<ResponseAuthenticationByGoogle> RegisterUserThroughAuthenticationByGoogle(ResponseAuthenticationByGoogle response)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == response.Email);

            if (user == null)
            {
                var role = await _db.Roles.FirstAsync(r => r.Name == "Customer");

                user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = response.Name!,
                    Email = response.Email!,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    PasswordHash = Guid.NewGuid().ToString(),
                    PasswordSalt = Guid.NewGuid().ToString(),
                    Roles = new List<Role> { role }                   // gán role có thằng user này 
                };

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
            }

            return new ResponseAuthenticationByGoogle
            {
                Id = user.Id,
                AvatarUrl = response.AvatarUrl,
                Email = response.Email,
                Name = response.Name
            };
        }


    }
}
