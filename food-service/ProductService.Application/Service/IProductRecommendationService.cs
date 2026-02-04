using food_service.ProductService.Application.DTOs.Request;
using food_service.ProductService.Application.DTOs.Response;

namespace food_service.ProductService.Application.Service
{
    public interface IProductRecommendationService
    {
        Task<List<ProductDTO>> ExecuteAsync(Guid idProduct);
    }
}
