using auth_services.AuthService.Application.DTOS;

namespace auth_services.AuthService.Application.Interfaces
{
    public interface IOutBoxMessage
    {
        Task CreateNewMessage(OutBoxMessageInternalDTO message);

        Task MartAsProccessed(Guid IdMessage);

        Task<List<OutBoxMessageInternalDTO>> GetMessage();
    }
}
