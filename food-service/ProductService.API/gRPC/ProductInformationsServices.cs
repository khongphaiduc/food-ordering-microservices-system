using food_service.ProductService.Infastructure.Models;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using productService.API.Protos;

namespace food_service.ProductService.API.gRPC
{
    public class ProductInformationsServices : ProductInfoGrpc.ProductInfoGrpcBase
    {
        private readonly FoodProductsDbContext _db;

        public ProductInformationsServices(FoodProductsDbContext foodProductsDbContext)
        {
            _db = foodProductsDbContext;
        }

        public override async Task<ProductDetailList> GetInformationProducts(ProductRequestList request, ServerCallContext context  )
        {
            var productIds = request.ProductId
                .Select(Guid.Parse)
                .ToList();


            var products = await _db.Products
                .Where(p => productIds.Contains(p.Id))
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .ToListAsync();


            var response = new ProductDetailList();

            foreach (var p in products)
            {
                var detail = new ProductDetail
                {
                    IdProduct = p.Id.ToString(),
                    IdCategory = p.CategoryId.ToString(),
                    Description = p.Description,
                    Price = (double)p.Price,
                    StatusCode = "200",
                    Name = p.Name

                };

                // Images
                detail.ProductImages.AddRange(
                    p.ProductImages.Select(i => new productService.API.Protos.ProductImage
                    {
                        IdImage = i.Id.ToString(),
                        UrlImage = i.ImageUrl
                    })
                );

                // Variants
                detail.ProductVariants.AddRange(
                    p.ProductVariants.Select(v => new productService.API.Protos.ProductVariant
                    {
                        IdVariant = v.Id.ToString(),
                        Name = v.Name,
                        ExtraPrice = (double)v.ExtraPrice,
                        TypeProduct = v.Name
                    })
                );

                response.ProductDetail.Add(detail);
            }

            return response;
        }



    }
}
