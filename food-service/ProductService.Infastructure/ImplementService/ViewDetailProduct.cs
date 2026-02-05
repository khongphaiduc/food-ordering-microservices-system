using food_service.ProductService.API.GlobalExceptions;
using food_service.ProductService.Application.DTOs.Response;
using food_service.ProductService.Application.Service;
using food_service.ProductService.Infastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace food_service.ProductService.Infastructure.ImplementService
{
    public class ViewDetailProduct : IViewDetailProduct
    {
        private readonly FoodProductsDbContext _db;
        private readonly IDistributedCache _redis;

        public ViewDetailProduct(FoodProductsDbContext foodProductsDbContext, IDistributedCache distributedCache)
        {
            _db = foodProductsDbContext;
            _redis = distributedCache;
        }

        public async Task<ProductDetailDTO> ExcuteAsync(Guid idProduct)
        {

            if (await _redis.GetStringAsync(idProduct.ToString()) != null)
            {
                var productCatche = await _redis.GetStringAsync(idProduct.ToString());
                var cacheProduct = JsonSerializer.Deserialize<ProductDetailDTO>(productCatche!);
                return cacheProduct!;
            }

            var product = await _db.Products.AsSplitQuery().Where(s => s.Id == idProduct).Select(s => new ProductDetailDTO
            {
                IdCategory = s.CategoryId,
                IdProduct = s.Id,
                Description = s.Description ?? "None",
                Price = s.Price,
                productImageDTOs = s.ProductImages.Where(t => t.ProductId == s.Id).Select(c => new ProductImageDTO
                {
                    IdImage = c.Id,
                    UrlImage = c.ImageUrl,
                }).ToList(),

                productVariantDTOs = s.ProductVariants.Where(b => b.ProductId == s.Id).Select(g => new ProductVariantDTO
                {
                    IdVariant = g.Id.ToString(),
                    Name = g.Name,
                    ExtraPrice = g.ExtraPrice,
                    TypeProduct = "Variant"

                }).ToList(),

            }).FirstOrDefaultAsync();

            if (product != null)
            {
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(DateTimeOffset.Now.AddHours(6));
                var productString = JsonSerializer.Serialize(product);
                await _redis.SetStringAsync(idProduct.ToString(), productString, options);
            }

            if (product == null)
            {
                return null;
            }

            return product;
        }
    }
}
