namespace payment_service.PaymentService.Domain.Enums
{
    public enum PaymentTransactionStatus
    {
        Initiated = 0,
        Requested = 1,
        Processing = 2,
        Succeeded = 3,
        Failed = 4,
        Cancelled = 5,
        Timeout = 6,
        Unknown = 7
    }
}
