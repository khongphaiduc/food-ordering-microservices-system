using auth_service.authservice.application.dtos;

namespace auth_service.authservice.application.InterfaceApplication
{
    public interface IUserLogin
    {
        public Task<bool> LoginHandler(RequestAccount request);
    }
}
