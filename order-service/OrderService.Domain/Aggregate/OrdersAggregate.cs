using order_service.OrderService.Domain.Entities;
using order_service.OrderService.Domain.OjectValue;

namespace order_service.OrderService.Domain.Aggregate
{
    public class OrdersAggregate
    {
        public Guid IdOrder { get; private set; }

        public Guid IdCustomer { get; private set; }

        public string Status { get; private set; } = null!;

        public Price TotalAmount { get; private set; }

        public string ShippingFee { get; private set; } = null!;  // tiền giao 

        public DiscountValue Discount { get; private set; } // giảm giá

        public Price FinalAmount { get; private set; }

        public string PaymentMethod { get; private set; } = null!;

        public DateTime CreateAt { get; private set; }

        public DateTime UpdateAt { get; private set; }

        private List<OrderItemsEntity> orderItemsEntities = new();

        public IReadOnlyList<OrderItemsEntity> OrderItemsEntities => orderItemsEntities.AsReadOnly();


        private List<OrderPaymentsEntity> orderPaymentsEntities = new();

        public IReadOnlyList<OrderPaymentsEntity> OrderPaymentsEntities => orderPaymentsEntities.AsReadOnly();

        public OrderDeliveryEntity deliveryEntity { get; private set; }


        internal OrdersAggregate(Guid idOrder, Guid idCustomer, string status, Price totalAmount, string shippingFee, DiscountValue discount, Price finalAmount, string paymentMethod, DateTime createAt, DateTime updateAt)
        {
            IdOrder = idOrder;
            IdCustomer = idCustomer;
            Status = status;
            TotalAmount = totalAmount;
            ShippingFee = shippingFee;
            Discount = discount;
            FinalAmount = finalAmount;
            PaymentMethod = paymentMethod;
            CreateAt = createAt;
            UpdateAt = updateAt;
        }

        public void AddOrderItem(OrderItemsEntity orderItem)
        {
            orderItemsEntities.Add(orderItem);
        }


        public void AddOrderPayment(OrderPaymentsEntity orderPayment)
        {
            orderPaymentsEntities.Add(orderPayment);
        }

    }
}
