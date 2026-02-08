using cart_service.CartService.Application.DTOs;

namespace cart_service.CartService.Application.Services
{
    public interface IGetCartForUser
    {
        Task<ResponseViewCartUser> Excute(Guid idUser);
    }
}
