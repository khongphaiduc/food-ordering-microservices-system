namespace order_service.OrderService.Appilcation.DTOs
{
    public class RequestViewOrderDetail
    {
        public Guid IdUser { get; set; }

        public Guid IdOrder { get; set; }
    }
}
