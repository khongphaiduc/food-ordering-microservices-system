using payment_service.PaymentService.Domain.Enums;

namespace payment_service.PaymentService.Application.DTOs
{
    public class OrderDTOInternal
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderCode { get; set; } = null!;
        public OrderStatus Status { get; set; }

    }
}
