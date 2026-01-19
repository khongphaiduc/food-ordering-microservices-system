namespace search_service.SearchService.API.GlobalException
{
    public class NotFoundProductToUpdateException : Exception
    {
        public NotFoundProductToUpdateException()
        {
        }

        public NotFoundProductToUpdateException(string? message) : base(message)
        {
        }
    }
}
