namespace search_service.SearchService.Application.Interface
{
    public interface ISuggestSearch
    {
        Task<List<string>> SuggestSearchAsync(string key);
    }
}
