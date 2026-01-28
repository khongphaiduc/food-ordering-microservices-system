using CommunityToolkit.HighPerformance.Helpers;
using food_service.ProductService.Application.DTOs.Request;
using food_service.ProductService.Application.Interface;
using food_service.ProductService.Application.Service;
using food_service.ProductService.Domain.Aggragate;
using food_service.ProductService.Domain.Entities;
using food_service.ProductService.Domain.Interface;
using food_service.ProductService.Domain.ValueOject;
using food_service.ProductService.Infastructure.Models;
using Microsoft.EntityFrameworkCore;
using Minio;

namespace food_service.ProductService.Infastructure.ImplementService
{
    public class UpdateProduct : IUpdateProduct
    {
        private readonly IProductRepository _product;
        private readonly FoodProductsDbContext _db;
        private readonly IMinIOFood _minIO;

        public UpdateProduct(IProductRepository productRepository, FoodProductsDbContext db, IMinIOFood minIOFood)
        {
            _product = productRepository;
            _db = db;
            _minIO = minIOFood;
        }


        // update image product 
        public async Task Excute(UpdateProductDTO productRequest)
        {

            var product = await _db.Products.Include(s => s.ProductVariants).Include(s => s.ProductImages).Where(s => s.Id == productRequest.IdProduct).FirstOrDefaultAsync();

            if (product == null) { return; }

            var listImage = product.ProductImages.Select(s => new ProductImagesEntity(s.Id, s.ProductId, s.ImageUrl, s.IsMain)).ToList();

            var listVariant = product.ProductVariants.Select(s => new ProductVariantEntity(s.Id, s.ProductId, new Name(s.Name), new Price(s.ExtraPrice), s.IsActive, s.CreatedAt, s.UpdatedAt)).ToList();

            var productAggregate = new ProductAggregate(product.CategoryId, product.Id, new(product.Name),
                new Domain.ValueOject.Price(product.Price), product.Description, product.IsAvailable,
                product.IsDeleted, product.CreatedAt, product.UpdatedAt, listImage, listVariant);


            if (productRequest.Description != null) productAggregate.ChangeDescription(productRequest.Description);

            if (productRequest.Price != null) productAggregate.ChangePrice(new Price(productRequest.Price.Value));

            if (productRequest.Name != null) productAggregate.ChangeName(new Name(productRequest.Name));



            // add new images
            if (productRequest.AddnewImagesProducts != null && productRequest.AddnewImagesProducts.Any())
            {
                foreach (var image in productRequest.AddnewImagesProducts)
                {
                    var nameImage = await _minIO.UploadAsync(image.images);
                    productAggregate.AddNewImage(ProductImagesEntity.CreateNewImage(productAggregate.Id, nameImage, image.IsMain));
                }
            }


            // delete imgae 
            if (productRequest.DeleteImage != null && productRequest.DeleteImage.Any())
            {
                foreach (var image in productAggregate.ProductImagesEntities)
                {
                    var imageName = image.ImageUrl;
                    await _minIO.DeleteAsync(imageName);
                }
            }



            await _product.UpdateProductAsync(productAggregate);
        }


    }
}
