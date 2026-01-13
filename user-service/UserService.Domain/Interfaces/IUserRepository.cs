using user_service.UserService.Domain.Aggregates;

namespace user_service.UserService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddNewUserAsync(UserAggregate userAggregate);

        Task<bool> UpdateUserAsync(UserAggregate userAggregate);

        Task<bool> IsEmailExistsAsync(string email);

        Task<UserAggregate> GetUserAggregatebyId(Guid id);
    }
}
