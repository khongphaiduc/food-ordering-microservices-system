using food_service.ProductService.Application.DTOs.Internals;

namespace food_service.ProductService.Application.Interface
{
    public interface IOutBoxPatternProduct
    {

        Task CreateNewMessage(OutboxMessageDTO message);

    }
}
