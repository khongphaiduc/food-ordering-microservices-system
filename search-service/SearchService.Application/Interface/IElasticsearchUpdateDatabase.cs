namespace search_service.SearchService.Application.Interface
{
    public interface IElasticsearchUpdateDatabase
    {
        Task UpdateDocumentFromDatabase(Guid id);
    }
}
