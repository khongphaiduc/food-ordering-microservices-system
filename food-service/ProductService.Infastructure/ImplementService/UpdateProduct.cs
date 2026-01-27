using food_service.ProductService.Application.Service;
using food_service.ProductService.Domain.Aggragate;
using food_service.ProductService.Domain.Entities;
using food_service.ProductService.Domain.Interface;
using food_service.ProductService.Domain.ValueOject;
using food_service.ProductService.Infastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace food_service.ProductService.Infastructure.ImplementService
{
    public class UpdateProduct : IUpdateProduct
    {
        private readonly IProductRepository _product;
        private readonly FoodProductsDbContext _db;

        public UpdateProduct(IProductRepository productRepository, FoodProductsDbContext db)
        {
            _product = productRepository;
            _db = db;
        }

        public async Task Excute(Guid IdProduct)
        {

            var product = await _db.Products.Include(s => s.ProductVariants).Include(s => s.ProductImages).Where(s => s.Id == IdProduct).FirstOrDefaultAsync();

            if (product == null) { return; }

            var listImage = product.ProductImages.Select(s => new ProductImagesEntity(s.Id, s.ProductId, s.ImageUrl, s.IsMain)).ToList();

            var listVariant = product.ProductVariants.Select(s => new ProductVariantEntity(s.Id, s.ProductId, new Name(s.Name), new Price(s.ExtraPrice), s.IsActive, s.CreatedAt, s.UpdatedAt)).ToList();

            var productAggregate = new ProductAggregate(product.CategoryId, product.Id, new(product.Name),
                new Domain.ValueOject.Price(product.Price), product.Description, product.IsAvailable,
                product.IsDeleted, product.CreatedAt, product.UpdatedAt, listImage, listVariant);

            await _product.UpdateProductAsync(productAggregate);
        }


    }
}
