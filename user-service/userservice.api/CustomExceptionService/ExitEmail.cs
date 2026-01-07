namespace user_service.userservice.api.CustomExceptionService
{
    public class ExitEmail : Exception
    {
        public ExitEmail()
        {
        }

        public ExitEmail(string? message) : base(message)
        {
        }
    }
}
