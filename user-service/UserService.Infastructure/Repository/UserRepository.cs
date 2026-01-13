using Microsoft.EntityFrameworkCore;
using user_service.userservice.infastructure.DBcontextService;
using user_service.userservice.infastructure.Models;
using user_service.UserService.Domain.Aggregates;
using user_service.UserService.Domain.Entities;
using user_service.UserService.Domain.Interfaces;

namespace user_service.UserService.Infastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly FoodUsersContext _db;

        public UserRepository(FoodUsersContext foodUsersContext)
        {
            _db = foodUsersContext;
        }

        public async Task<bool> AddNewUserAsync(UserAggregate userAggregate)
        {

            var user = new User()
            {
                Id = userAggregate.Id,
                FullName = userAggregate.Username!,
                Email = userAggregate.Email.Value,
                PhoneNumber = userAggregate.PhoneNumberUser.value,
                IsActive = userAggregate.IsActive,
                CreatedAt = userAggregate.CreatedAt,
                UpdatedAt = userAggregate.UpdatedAt,
                UserAddresses = userAggregate.UserAddresses.Select(address => new UserAddress
                {
                    Id = address.Id,
                    UserId = userAggregate.Id,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    District = address.District,
                    PostalCode = address.PostalCode,
                    IsDefault = address.IsDefault,
                    CreatedAt = address.CreatedAt,
                    UpdatedAt = address.UpdatedAt
                }).ToList()
            };

            await _db.Users.AddAsync(user);
            return await _db.SaveChangesAsync() > 0;
        }

        // lấy thông tin user và map sang UserAggregate
        public async Task<UserAggregate> GetUserAggregatebyId(Guid id)
        {
            var user = await _db.Users.Include(s => s.UserAddresses).FirstOrDefaultAsync(u => u.Id == id);
            return UserAggregate.Rehydrate(
                user!.Id,
                user.FullName!,
                new Domain.ValueObjects.Email(user.Email!),
                new Domain.ValueObjects.PhoneNumber(user.PhoneNumber!),
                user.IsActive,
                user.CreatedAt,
                user.UpdatedAt,
                user.UserAddresses.Select(address => UserAddresses.Rehydrate(
                    address.Id,
                    address.UserId,
                    address.AddressLine1!,
                    address.AddressLine2!,
                    address.City!,
                    address.District!,
                    address.PostalCode!,
                    address.IsDefault,
                    address.CreatedAt,
                    address.UpdatedAt
                )).ToList()
            );
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user != null;
        }

        // cập nhật các thong tin cơ bản của user và địa chỉ
        public async Task<bool> UpdateUserAsync(UserAggregate userAggregate)
        {
            var user = await _db.Users.Include(s => s.UserAddresses).FirstOrDefaultAsync(u => u.Id == userAggregate.Id);


            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FullName = userAggregate.Username!;
            user.Email = userAggregate.Email.Value;
            user.PhoneNumber = userAggregate.PhoneNumberUser.value;
            user.IsActive = userAggregate.IsActive;
            user.UpdatedAt = userAggregate.UpdatedAt;

            foreach (var item in userAggregate.UserAddresses)
            {
                if (!user.UserAddresses.Any(s => s.Id == item.Id))
                {
                    user.UserAddresses.Add(new UserAddress()
                    {
                        Id = item.Id,
                        UserId = userAggregate.Id,
                        AddressLine1 = item.AddressLine1,
                        AddressLine2 = item.AddressLine2,
                        City = item.City,
                        District = item.District,
                        PostalCode = item.PostalCode,
                        IsDefault = item.IsDefault,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    });
                }
                else
                {
                    var add = user.UserAddresses.FirstOrDefault(s => s.Id == item.Id);
                    if (add != null)
                    {
                        add.AddressLine1 = item.AddressLine1;
                        add.AddressLine2 = item.AddressLine2;
                        add.City = item.City;
                        add.District = item.District;
                        add.PostalCode = item.PostalCode;
                        add.IsDefault = item.IsDefault;
                        add.UpdatedAt = item.UpdatedAt;
                    }
                }

            }
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
