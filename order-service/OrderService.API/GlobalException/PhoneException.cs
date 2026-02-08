using System.Runtime.Serialization;

namespace order_service.OrderService.API.GlobalException
{
    public class PhoneException : Exception
    {
        public PhoneException()
        {
        }

        public PhoneException(string? message) : base(message)
        {
        }
    }
}
