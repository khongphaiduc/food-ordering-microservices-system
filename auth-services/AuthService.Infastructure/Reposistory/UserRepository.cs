using auth_services.AuthService.API.CustomExceptions;
using auth_services.AuthService.Domain.Aggregate;
using auth_services.AuthService.Domain.Interface;
using auth_services.AuthService.Infastructure.DbContextAuth;
using auth_services.AuthService.Infastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace auth_services.AuthService.Infastructure.Reposistory
{
    public class UserRepository : IUserRepository
    {
        private readonly FoodAuthContext _db;

        public UserRepository(FoodAuthContext foodAuthContext)
        {
            _db = foodAuthContext;
        }

        // thêm user 
        public async Task<bool> AddNewUser(UserAggregate userAggregate)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(s => s.Name == "Customer") ?? throw new NotfoundExceptions("Not found role User");

            // từ UserAggregate chuyển thành User model để lưu vào database
            var users = new User()
            {
                Id = userAggregate.Id,
                Username = userAggregate.Username.Value,
                Email = userAggregate.Email.EmailAdress,
                PasswordHash = userAggregate.PasswordHash,
                PasswordSalt = userAggregate.PasswordSalt,
                IsActive = userAggregate.IsActive,
                CreatedAt = userAggregate.CreatedAt,
                UpdatedAt = userAggregate.UpdatedAt,
            };
            users.Roles.Add(role);
            await _db.Users.AddAsync(users);
            return await _db.SaveChangesAsync() > 0;
        }

        public Task<bool> IsExitUser(string email)
        {
            return _db.Users.AnyAsync(u => u.Email == email);
        }
    }
}
