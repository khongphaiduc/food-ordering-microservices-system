using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using search_service.Models;
using search_service.SearchService.API.GlobalException;
using search_service.SearchService.Application.IndexModel;
using search_service.SearchService.Application.Interface;

namespace search_service.SearchService.Infastructure.ImplementServices
{
    public class ElasticsearchUpdateDatabase : IElasticsearchUpdateDatabase
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private readonly FoodProductsDbContext _db;

        public ElasticsearchUpdateDatabase(FoodProductsDbContext foodProductsDbContext, ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
            _db = foodProductsDbContext;
        }

        public async Task UpdateDocumentFromDatabase(Guid id)
        {

            var product = await _db.Products.Include(s => s.ProductImages).Where(t => t.Id == id).FirstOrDefaultAsync();

            if (product == null)
            {
                throw new NotFoundProductToUpdateException($"Not found product Id {id}");
            }

            var productDocument = new ProductDoc
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryID = product.CategoryId,
                CreateAt = product.CreatedAt,
                UpdateAt = product.UpdatedAt,
                imagesProduct = product.ProductImages.Select(c => new ImageDoc
                {
                    Id = c.Id,
                    UrlImage = c.ImageUrl,
                    IsMain = c.IsMain,
                }).ToList()
            };

            var result = await _elasticsearchClient.IndexAsync<ProductDoc>(productDocument, s => s.Index("products").Id(productDocument.Id));

        }
    }
}
