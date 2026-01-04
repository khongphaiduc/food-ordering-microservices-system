using auth_service.authservice.application.dtos;

namespace auth_service.authservice.application.InterfaceApplication
{
    public interface IAuthenticationByGoogle
    {

        Task<ResponseAuthenticationByGoogle> RegisterUserThroughAuthenticationByGoogle(ResponseAuthenticationByGoogle response);


    }
}
