using cart_service.CartService.Application.DTOs;

namespace cart_service.CartService.Application.Services
{
    public interface IUpdateCartFood
    {
        Task Excute(RequestUpdateCartFood request);
    }
}
