namespace user_service.UserService.API.Middlwares
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException()
        {
        }

        public NotFoundUserException(string? message) : base(message)
        {
        }
    }
}
