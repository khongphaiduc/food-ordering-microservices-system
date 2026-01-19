using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Microsoft.Extensions.Caching.Distributed;
using search_service.SearchService.Application.IndexModel;
using search_service.SearchService.Application.Interface;
using System.Text.Json;

namespace search_service.SearchService.Infastructure.ImplementServices
{
    public class Elasticsearch : IElasticsearch
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private readonly IDistributedCache _distributeCache;

        public Elasticsearch(ElasticsearchClient elasticsearchClient, IDistributedCache distributedCache)
        {
            _elasticsearchClient = elasticsearchClient;
            _distributeCache = distributedCache;
        }

        public async Task<bool> AddNewProduct(ProductDoc product)
        {
            var result = await _elasticsearchClient.IndexAsync(product, s => s.Index("products").Id(product.Id));

            return result.IsValidResponse;
        }

        public async Task<bool> DeleteProduct(ProductDoc product)
        {
            var response = await _elasticsearchClient.DeleteAsync<ProductDoc>(product.Id.ToString(), d => d.Index("products"));
            return response.IsValidResponse;
        }

        public async Task<bool> GetProduct(Guid Id)
        {
            var response = await _elasticsearchClient.GetAsync<ProductDoc>(Id.ToString(), g => g.Index("products"));
            return response.IsValidResponse;
        }

        public async Task<List<ProductDoc>> SearchByKey(string key, int indexPage)
        {
            var cacheKey = $"search:{key}:page:{indexPage}";

            var dataProduct = await _distributeCache.GetStringAsync(cacheKey);

            if (dataProduct != null)
            {
                var finalContent = JsonSerializer.Deserialize<List<ProductDoc>>(dataProduct);

                return finalContent;
            }

            var size = 10;
            var number = (indexPage - 1) * 10;

            var resukt = await _elasticsearchClient.SearchAsync<ProductDoc>(s => s.Index("products")
                                                                     .From(number)
                                                                     .Size(size)
                                                                     .Query(s => s.MultiMatch(t => t.Fields("name").Fields("description").Query(key))));
            var productDocument = resukt.Documents.ToList();

            var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(50));
            var contents = JsonSerializer.Serialize(productDocument);

            await _distributeCache.SetStringAsync(cacheKey, contents, option);

            return productDocument;
        }

    }
}
