namespace payment_service.PaymentService.Domain.Enums
{
    public enum OrderStatus
    {
        PENDING = 1,
        CONFIRMED = 2,
        PAID = 3,
        CANCELLED = 4,
        DELIVERED = 5
    }
}
