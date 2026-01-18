using search_service.SearchService.Application.IndexModel;
namespace search_service.SearchService.Application.Interface
{
    public interface IGetListProduct
    {
        Task<List<ProductDoc>> ExcuteAsync();
    }
}
