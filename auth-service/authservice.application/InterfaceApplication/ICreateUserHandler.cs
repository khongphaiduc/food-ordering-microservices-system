using auth_service.authservice.application.dtos;

namespace auth_service.authservice.application.InterfaceApplication
{
    public interface ICreateUserHandler
    {
        Task<RequestAccount> HandleCreateUser(RequestAccount register);
    }
}
