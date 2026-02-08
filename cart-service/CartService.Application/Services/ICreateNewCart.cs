using cart_service.CartService.Application.DTOs;

namespace cart_service.CartService.Application.Services
{
    public interface ICreateNewCart
    {
        Task<Guid> Excute(RequestCreateNewCartUser request); // id cart

    }
}
