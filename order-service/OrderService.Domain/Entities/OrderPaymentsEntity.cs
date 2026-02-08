namespace order_service.OrderService.Domain.Entities
{
    public class OrderPaymentsEntity
    {
        public Guid IdOrderPayment { get; private set; }

        public Guid IdOrder { get; private set; }

        public string TransactionId { get; private set; } = null!; // mã code do cổng thanh toán trả về

        public DateTime PaidAt { get; private set; }  //  thời điểm ngân hàng báo thành toán thành công

        public DateTime CreateAt { get; private set; }  // lúc tạo đơn 


        public static OrderPaymentsEntity CreateOrderPayment(Guid idOrder, string transactionId, DateTime paidAt)
        {
            return new OrderPaymentsEntity(
                Guid.NewGuid(),
                idOrder,
                transactionId,
                paidAt,
                DateTime.UtcNow
            );
        }

        internal OrderPaymentsEntity(Guid idOrderPayment, Guid idOrder, string transactionId, DateTime paidAt, DateTime createAt)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("TransactionId is required");

            IdOrderPayment = idOrderPayment;
            IdOrder = idOrder;
            TransactionId = transactionId;
            PaidAt = paidAt;
            CreateAt = createAt;
        }

        private OrderPaymentsEntity()
        {
        }

    }
}
