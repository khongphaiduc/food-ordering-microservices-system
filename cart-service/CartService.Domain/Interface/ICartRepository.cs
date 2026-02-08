using cart_service.CartService.Domain.Aggregate;

namespace cart_service.CartService.Domain.Interface
{
    public interface ICartRepository
    {
        Task<Guid> CreateCartAsync(CartAggregate cartAggregate);
        Task<CartAggregate?> GetCartByUserIdAsync(Guid userId);
        Task UpdateCartAsync(CartAggregate cartAggregate);
    }
}
