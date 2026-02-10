using Microsoft.Identity.Client;
using payment_service.PaymentService.Domain.Entities;
using payment_service.PaymentService.Domain.Enums;

namespace payment_service.PaymentService.Domain.Aggregate
{
    public class PaymentAggregate
    {
        public Guid IdPayment { get; private set; }

        public Guid IdOrder { get; private set; }

        public Guid IdUser { get; private set; }

        public decimal Amount { get; private set; }

        public UnitMoney Currency { get; private set; }

        public PaymentMethod PaymentMethods { get; private set; }

        public PaymentMethod? Provider { get; private set; }

        public string? TransactionId { get; private set; }

        public PaymentStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? UpdatedAt { get; private set; }

        private List<PaymentTransactionEnity> Refunds = new List<PaymentTransactionEnity>();

        public IReadOnlyList<PaymentTransactionEnity> GetRefunds => Refunds.AsReadOnly();


        public static PaymentAggregate CreateNewPayment(Guid IdOrder, Guid IdUser, decimal Amount, UnitMoney unitMoney, PaymentMethod paymentMethod, string? TransactionId, PaymentStatus paymentStatus)
        {
            return new PaymentAggregate
            {
                IdOrder = IdOrder,
                IdUser = IdUser,
                Amount = Amount,
                Currency = unitMoney,
                PaymentMethods = paymentMethod,
                TransactionId = TransactionId,
                Status = paymentStatus,
                CreatedAt = DateTime.UtcNow
            };
        }

        internal PaymentAggregate(Guid idPayment, Guid idOrder, Guid idUser, decimal amount, UnitMoney currency, PaymentMethod paymentMethods, PaymentMethod? provider, string? transactionId, PaymentStatus status, DateTime createdAt, DateTime? updatedAt, List<PaymentTransactionEnity> refunds)
        {
            IdPayment = idPayment;
            IdOrder = idOrder;
            IdUser = idUser;
            Amount = amount;
            Currency = currency;
            PaymentMethods = paymentMethods;
            Provider = provider;
            TransactionId = transactionId;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Refunds = refunds;
        }

        private PaymentAggregate()
        {
        }

        public void AddTransaction(PaymentTransactionEnity paymentTransaction)
        {
            Refunds.Add(paymentTransaction);
        }

    }
}
