using payment_service.PaymentService.Domain.Enums;

namespace payment_service.PaymentService.Domain.Entities
{
    public class PaymentTransactionEnity
    {
        public Guid IdPaymentTrancsaction { get; private set; }

        public Guid IdPayment { get; private set; }

        public string? ProviderTransactionId { get; private set; }

        public PaymentTransactionStatus? Status { get; private set; }

        public string? RawResponse { get; private set; }

        public DateTime CreatedAt { get; private set; }


    }
}
