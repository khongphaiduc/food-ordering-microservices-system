using search_service.SearchService.Application.IndexModel;

namespace search_service.SearchService.Application.Interface
{
    public interface IElasticsearch
    {
        Task<bool> AddNewProduct(ProductDoc product);
        Task<bool> DeleteProduct(ProductDoc product);
        Task<bool> GetProduct(Guid Id);

        Task<List<ProductDoc>> SearchByKey(string key, int indexPage); 
    }
}
