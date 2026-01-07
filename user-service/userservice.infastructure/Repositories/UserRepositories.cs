
using Microsoft.EntityFrameworkCore;
using user_service.userservice.api.CustomExceptionService;
using user_service.userservice.domain.entity;
using user_service.userservice.domain.interfaces;
using user_service.userservice.infastructure.DBcontextService;
using user_service.userservice.infastructure.Models;

namespace user_service.userservice.infastructure.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly FoodUsersContext _db;

        public UserRepositories(FoodUsersContext foodUsersContext)
        {
            _db = foodUsersContext;
        }

        // thêm địa chỉ cho thằng user
        public async Task<bool> AddAdressForUser(AddressUserEntity addressUser)
        {
            await _db.UserAddresses.AddAsync(new UserAddress()
            {
                Id = addressUser.Id,
                UserId = addressUser.UserId,
                AddressLine1 = addressUser.AddressLine1.Value,
                AddressLine2 = addressUser.AddressLine2,
                City = addressUser.City.Value,
                District = addressUser.District,
                PostalCode = addressUser.PostalCode.Value,
                CreatedAt = addressUser.CreatedAt,
                UpdatedAt = addressUser.UpdatedAt,
            });
            return await _db.SaveChangesAsync() > 0;
        }

        // thêm user
        public async Task<bool> AddUser(UsersEntity user)
        {
            await _db.Users.AddAsync(new User()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.EmailAddress?.Value,
                PhoneNumber = user.PhoneNumbers.Number,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsEmailExsit(string email)
        {

            var user = await _db.Users.FirstOrDefaultAsync(s => s.Email == email);

            return user != null;
        }

        public Task<bool> UpdateAdressForUser(AddressUserEntity addressUser)
        {

            throw new NotImplementedException();
        }

        //  update thông tin của thằng user 
        public async Task<bool> UpdateUser(UsersEntity user)
        {
            var updateUser = await _db.Users.FirstOrDefaultAsync(s => s.Id == user.Id);

            if (updateUser == null)
            {
                throw new NotFoundException("User not found");
            }

            updateUser.UpdatedAt = DateTime.Now;
            updateUser.FullName = user.FullName;
            updateUser.Email = user.EmailAddress?.Value;
            updateUser.PhoneNumber = user.PhoneNumbers.Number;

            _db.Users.Update(updateUser);

            return await _db.SaveChangesAsync() > 0;
        }
    }
}
