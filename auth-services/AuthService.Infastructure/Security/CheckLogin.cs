using auth_services.AuthService.Application.DTOS;
using auth_services.AuthService.Application.Interfaces;
using auth_services.AuthService.Application.Service;
using auth_services.AuthService.Infastructure.DbContextAuth;
using Microsoft.EntityFrameworkCore;

namespace auth_services.AuthService.Infastructure.Security
{
    public class CheckLogin : ICheckLogin
    {
        private readonly FoodAuthContext _db;
        private readonly IHashPassword _IhashPassword;

        public CheckLogin(FoodAuthContext foodAuthContext, IHashPassword hashPassword)
        {
            _db = foodAuthContext;
            _IhashPassword = hashPassword;
        }

        public async Task<bool> IsUserLoginAsync(RequestUserLogin user)
        {
            var realUserInDataBase = await _db.Users.Where(s => s.Email == user.Email).Select(s => new
            {
                passwordHash = s.PasswordHash,
                paswordSalt = s.PasswordSalt,
            }).FirstOrDefaultAsync();

            if (realUserInDataBase == null)
            {
                return false;
            }

            var s = _IhashPassword.HandleHashPassword(user.Password, realUserInDataBase.paswordSalt);

            if (s == realUserInDataBase.passwordHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
