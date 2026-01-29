
using CommunityToolkit.HighPerformance.Helpers;
using food_service.ProductService.API.GlobalExceptions;
using food_service.ProductService.Application.Interface;
using food_service.ProductService.Domain.Aggragate;
using food_service.ProductService.Domain.Entities;
using food_service.ProductService.Domain.Interface;
using food_service.ProductService.Domain.ValueOject;
using food_service.ProductService.Infastructure.Models;
using food_service.ProductService.Infastructure.ProducerRabbitMQ;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace food_service.ProductService.Infastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly FoodProductsDbContext _db;
        private readonly FoodProducer _workerFood;
        private readonly IOutBoxPatternProduct _outBoxpattern;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(FoodProductsDbContext foodProductsDbContext, FoodProducer foodProducer, IOutBoxPatternProduct outBoxPatternProduct, ILogger<ProductRepository> logger)
        {
            _db = foodProductsDbContext;
            _workerFood = foodProducer;
            _outBoxpattern = outBoxPatternProduct;
            _logger = logger;
        }

        public async Task<bool> AddProductAsync(ProductAggregate product)
        {
            _logger.LogInformation("Begin add new product ");
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var producModel = new Product
                {
                    Id = product.Id,
                    CategoryId = product.CategoryId,
                    Name = product.NameProduct.Value,
                    Description = product.Description,
                    Price = product.PriceProduct.Value,
                    IsAvailable = product.IsAvailable,
                    IsDeleted = product.IsDeleted,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                    ProductImages = product.ProductImagesEntities.Select(s => new ProductImage
                    {
                        Id = s.Id,
                        ProductId = s.ProductId,
                        ImageUrl = s.ImageUrl,
                        IsMain = s.IsMain,
                    }).ToList(),

                    ProductVariants = product.ProductVariantEntities.Select(q => new ProductVariant
                    {
                        Id = q.Id,
                        ProductId = q.ProductId,
                        Name = q.VariantName.Value,
                        CreatedAt = q.CreateAt,
                        ExtraPrice = q.ExtraPrice.Value,
                        IsActive = q.IsActive,
                        UpdatedAt = q.UpdateAt

                    }).ToList()
                };


                var productDTP = new ProductInternalDTO
                {
                    Id = producModel.Id,
                    Description = producModel.Description,
                    IdCategory = producModel.CategoryId,
                    Name = producModel.Name,
                    Price = producModel.Price,
                    UpdateAt = producModel.UpdatedAt,
                    CreateAt = producModel.CreatedAt,

                    productImageInternalDTOs = product.ProductImagesEntities.Select(s => new ProductImageInternalDTO
                    {
                        Id = s.Id,
                        IsMain = s.IsMain,
                        URLImage = s.ImageUrl,
                    }).ToList(),


                    productVarientInternalDTOs = product.ProductVariantEntities.Select(f => new ProductVarientInternalDTO
                    {
                        CreateAt = f.CreateAt,
                        Extra_Price = f.ExtraPrice.Value,
                        Name = f.VariantName.Value,
                        IdProduct = f.ProductId,
                        UpdateAt = f.UpdateAt,

                    }).ToList(),
                };

                // ghi vào base 
                await _db.Products.AddAsync(producModel);


                // ghi vào OutBoxMessage
                await _outBoxpattern.CreateNewMessage(new Application.DTOs.Internals.OutboxMessageDTO
                {
                    Id = Guid.NewGuid(),
                    Type = "ProductCreated",
                    PayLoad = JsonSerializer.Serialize(productDTP),
                    IsProcesced = false,
                    CreateAt = DateTime.UtcNow,
                });


                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("write new product into OutBoxTable Successful");
                return true;
            }
            catch (Exception s)
            {

                await transaction.RollbackAsync();
                _logger.LogError($"Error in Repository Create new Product: {s}");
                return false;
            }
        }

        public async Task<ProductAggregate> GetProductByIdAsync(Guid productId)
        {
            var product = await _db.Products.Include(s => s.ProductVariants).Include(s => s.ProductImages).FirstOrDefaultAsync(s => s.Id == productId);
            if (product != null)
            {
                var productAggregate = new ProductAggregate(
                 product.Id,
                 product.CategoryId,
                 new Name(product.Name),
                 new Price(product.Price),
                 product.Description,
                 product.IsAvailable,
                 product.IsDeleted,
                 product.CreatedAt,
                 product.UpdatedAt,
                 product.ProductImages.Select(s => new ProductImagesEntity(s.Id, s.ProductId, s.ImageUrl, s.IsMain)).ToList(),
                 product.ProductVariants.Select(pv => new ProductVariantEntity(pv.Id, pv.ProductId, new Name(pv.Name), new Price(pv.ExtraPrice), pv.IsActive, pv.CreatedAt, pv.UpdatedAt)).ToList()
                );
                return productAggregate;
            }
            else
            {
                throw new ProductNotFoundException($"ProductDTO with id {productId} not found");
            }
        }

        // cập nhật product aggregate
        public async Task UpdateProductAsync(ProductAggregate product)
        {

            _logger.LogInformation("Start update product ");
            var productBase = await _db.Products.Include(s => s.ProductVariants).Include(s => s.ProductImages).FirstOrDefaultAsync(p => p.Id == product.Id);


            if (productBase == null)
            {
                _logger.LogError($"Can not  find product with Id {product.Id}");
                throw new ProductNotFoundException($"ProductDTO with id {product.Id} not found");
            }
            else
            {
                productBase.Name = product.NameProduct.Value;
                productBase.Description = product.Description;
                productBase.Price = product.PriceProduct.Value;
                productBase.IsAvailable = product.IsAvailable;
                productBase.UpdatedAt = product.UpdatedAt;
                productBase.IsDeleted = product.IsDeleted;




                foreach (var image in product.ProductImagesEntities)
                {
                    if (!productBase.ProductImages.Any(s => s.Id == image.Id))   // db không có , request có  => thêm 
                    {
                        _db.ProductImages.Add(new ProductImage()
                        {
                            Id = image.Id,
                            ProductId = image.ProductId,
                            ImageUrl = image.ImageUrl,
                            IsMain = image.IsMain
                        });
                    }
                    else                                                        // db có , requets có => update 
                    {
                        var imageItem = productBase.ProductImages.FirstOrDefault(s => s.Id == image.Id);

                        if (imageItem != null)
                        {
                            imageItem.ImageUrl = image.ImageUrl;
                            imageItem.IsMain = image.IsMain;
                        }

                    }

                }

                // Db có , reuqest không có  => xóa
                //  var ImageProductId = product.ProductImagesEntities.Select(s => s.Id).ToList();
                //var listImageRemove = productBase.ProductImages.Where(s => !ImageProductId.Any(t => s.Id == t)).ToList();
                //_db.RemoveRange(listImageRemove);




                var result = await _db.SaveChangesAsync() > 0;
                if (result)
                {
                    _logger.LogInformation($"update product id  {product.Id} Successful");
                }
                else
                {
                    _logger.LogInformation($"update product id  {product.Id} fail");
                }

            }

        }
    }
}
