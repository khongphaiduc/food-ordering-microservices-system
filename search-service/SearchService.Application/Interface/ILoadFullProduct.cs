namespace search_service.SearchService.Application.Interface
{
    public interface ILoadFullProduct
    {
        Task<bool> LoadFullProductAsync();
    }
}
