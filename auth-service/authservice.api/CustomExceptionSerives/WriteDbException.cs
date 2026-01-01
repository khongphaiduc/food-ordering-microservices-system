namespace auth_service.authservice.api.CustomExceptionSerives
{
    public class WriteDbException : Exception
    {
        public WriteDbException()
        {
        }

        public WriteDbException(string? message) : base(message)
        {
        }
    }
}
