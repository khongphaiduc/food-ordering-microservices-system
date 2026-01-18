namespace food_service.ProductService.API.GlobalExceptions
{
    public class NotFoundCategoryException : Exception
    {
        public NotFoundCategoryException()
        {
        }

        public NotFoundCategoryException(string? message) : base(message)
        {
        }
    }
}
