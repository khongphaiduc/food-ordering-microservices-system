using auth_service.authservice.domain.entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace auth_service.authservice.domain.Interfaces
{
    public interface IUserRepositories
    {
        public Task<bool> CreateAccount(UserEntity user); // 1/1/2026

        public Task<UserEntity?> GetUserByEmail(string email);   // lấy email 


        public Task<string> GetSalt(string email); 

        public Task<string> GetPasswordHash(string email);
    }
}