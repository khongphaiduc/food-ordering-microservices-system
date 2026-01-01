using System.Reflection.Metadata.Ecma335;

namespace auth_service.authservice.api.CustomExceptionSerives
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}