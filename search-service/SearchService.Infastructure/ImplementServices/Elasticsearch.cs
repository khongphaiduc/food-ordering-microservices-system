using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using search_service.SearchService.Application.IndexModel;
using search_service.SearchService.Application.Interface;

namespace search_service.SearchService.Infastructure.ImplementServices
{
    public class Elasticsearch : IElasticsearch
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        public Elasticsearch(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
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
    }
}
