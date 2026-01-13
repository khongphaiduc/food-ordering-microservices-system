using user_service.UserService.Application.DTOS;

namespace user_service.UserService.Application.Services
{
    public interface IUserProfile
    {
        Task<bool> UserProfilHandle(RequestUserProfile requestUserProfile);
    }
}
