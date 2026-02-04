using food_service.ProductService.Application.DTOs.Response;
using food_service.ProductService.Application.Service;
using food_service.ProductService.Infastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace food_service.ProductService.Infastructure.ImplementService
{
    public class ProductRecommendationService : IProductRecommendationService
    {
        private readonly FoodProductsDbContext _db;

        public ProductRecommendationService(FoodProductsDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProductDTO>> ExecuteAsync(Guid IdCategory)
        {

            var listProductRecommendation = await _db.Products
                .Where(p => p.CategoryId != IdCategory )
                .OrderBy(p => Guid.NewGuid())
                .Take(8)
                .Select(p => new ProductDTO
                {
                    IdCategory = p.CategoryId,
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    UrlImageMain = p.ProductImages.Where(img => img.IsMain).Select(img => img.ImageUrl).FirstOrDefault() ?? "https://img6.thuthuatphanmem.vn/uploads/2022/04/16/anh-rose-blackpink-cute-xinh_042754601.jpg",
                    IsAvailable = p.IsAvailable,
                    Decriptions = p.Description
                })
                .ToListAsync();
            return listProductRecommendation;
        }


    }
}
