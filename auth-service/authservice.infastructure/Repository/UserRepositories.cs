using auth_service.authservice.domain.entities;
using auth_service.authservice.domain.Interfaces;
using auth_service.authservice.domain.value_object;
using auth_service.authservice.infastructure.dbcontexts;
using Microsoft.EntityFrameworkCore;

namespace auth_service.authservice.infastructure.Repository
{
    public class UserRepositories : IUserRepositories
    {
        private readonly FoodAuthContext _dbcontext;
        private readonly ILogger<UserRepositories> _Ilogger;

        public UserRepositories(FoodAuthContext dbcontext, ILogger<UserRepositories> logger)
        {
            _dbcontext = dbcontext;
            _Ilogger = logger;
        }

        // create new acccount 
        public async Task<bool> CreateAccount(UserEntity user)
        {
            _Ilogger.LogInformation("ĐANG TẠO USER ");

            await _dbcontext.Users.AddAsync(new Models.User()
            {
                Id = Guid.NewGuid(),
                Email = user.Email.EmailAdress,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            });

            return _dbcontext.SaveChanges() > 0;
        }

        public async Task<string> GetPasswordHash(string email)
        {
            
            var passwordHash = await _dbcontext.Users
                .Where(u => u.Email == email)
                .Select(u => u.PasswordHash)
                .FirstOrDefaultAsync();
            return passwordHash!;
        }

        public async Task<string> GetSalt(string email)
        {
            var salt = await _dbcontext.Users
                .Where(u => u.Email == email)
                .Select(u => u.PasswordSalt)
                .FirstOrDefaultAsync();

            return salt!;
        }

        // get a user by email
        public async Task<UserEntity?> GetUserByEmail(string email)
        {
            var existEmail = await _dbcontext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (existEmail == null)
            {
                _Ilogger.LogInformation("Email does not exist: {Email}", email);
                return null;
            }

            return new UserEntity
            {
                Id = existEmail.Id,
                Username = existEmail.Username,
                Email = new Email(existEmail.Email),
                PasswordHash = existEmail.PasswordHash,
                PasswordSalt = existEmail.PasswordSalt,
                IsActive = existEmail.IsActive,
                CreatedAt = existEmail.CreatedAt,
                UpdatedAt = existEmail.UpdatedAt
            };
        }

    }
}
