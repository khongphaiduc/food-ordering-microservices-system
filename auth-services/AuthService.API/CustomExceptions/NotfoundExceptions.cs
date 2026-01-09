namespace auth_services.AuthService.API.CustomExceptions
{
    public class NotfoundExceptions : Exception
    {

        public NotfoundExceptions(string? message) : base(message)
        {
        }
    }
}
