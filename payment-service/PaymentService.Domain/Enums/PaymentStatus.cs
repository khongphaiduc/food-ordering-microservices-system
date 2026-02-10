namespace payment_service.PaymentService.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 0,
        Processing = 1,
        Succeeded = 2,
        Failed = 3,
        Cancelled = 4,
        Expired = 5,
        Refunded = 6,
        PartiallyRefunded = 7
    }
}
