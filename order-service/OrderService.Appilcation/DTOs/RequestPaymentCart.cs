namespace order_service.OrderService.Appilcation.DTOs
{
    public class RequestPaymentCart
    {
        public Guid IdCart { get; set; }

        public int PaymentMethod { get; set; }
    }
}
