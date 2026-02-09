using order_service.OrderService.Domain.Entities;
using order_service.OrderService.Domain.Enums;
using order_service.OrderService.Domain.OjectValue;

namespace order_service.OrderService.Domain.Aggregate
{
    public class OrdersAggregate
    {
        public Guid IdOrder { get; private set; }
        public Guid IdCustomer { get; private set; }

        public OrderStatus Status { get; private set; }

        public Price TotalAmount { get; private set; }
        public decimal ShippingFee { get; private set; }      // phí giao hàng
        public DiscountValue Discount { get; private set; }   // giảm giá
        public Price FinalAmount { get; private set; }

        public PaymentMethod PaymentMethod { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private readonly List<OrderItemsEntity> orderItemsEntities = new();
        public IReadOnlyList<OrderItemsEntity> OrderItemsEntities => orderItemsEntities.AsReadOnly();

        private readonly List<OrderPaymentsEntity> orderPaymentsEntities = new();
        public IReadOnlyList<OrderPaymentsEntity> OrderPaymentsEntities => orderPaymentsEntities.AsReadOnly();

        public OrderDeliveryEntity? Delivery { get; private set; }

        #region Constructors


        internal OrdersAggregate(
            Guid idOrder,
            Guid idCustomer,
            OrderStatus status,
            decimal shippingFee,
            DiscountValue discount,
            PaymentMethod paymentMethod,
            DateTime createdAt
        )
        {
            IdOrder = idOrder;
            IdCustomer = idCustomer;
            Status = status;

            ShippingFee = shippingFee;
            Discount = discount;
            PaymentMethod = paymentMethod;

            TotalAmount = new Price(0);
            FinalAmount = new Price(shippingFee - discount.Value);

            CreatedAt = createdAt;
            UpdatedAt = createdAt;
        }


        private OrdersAggregate() { }

        #endregion

        #region Factory

        public static OrdersAggregate CreateNewOrders(
            Guid userId,
            decimal shippingFee,
            decimal discount,
            PaymentMethod paymentMethod
        )
        {
            return new OrdersAggregate(
                Guid.NewGuid(),
                userId,
                OrderStatus.PENDING,
                shippingFee,
                new DiscountValue(discount),
                paymentMethod,
                DateTime.UtcNow
            );
        }

        #endregion

        #region Business Methods

        public void AddOrderItem(OrderItemsEntity orderItem)
        {
            if (Status != OrderStatus.PENDING)
                throw new InvalidOperationException("Không thể thêm item khi order không ở trạng thái Pending");

            orderItemsEntities.Add(orderItem);
            RecalculateAmount();
        }

        public void AddOrderPayment(OrderPaymentsEntity orderPayment)
        {
            if (Status == OrderStatus.CANCELLED)
                throw new InvalidOperationException("Order đã bị hủy, không thể thanh toán");

            orderPaymentsEntities.Add(orderPayment);
        }

        public void SetDelivery(OrderDeliveryEntity delivery)
        {
            Delivery = delivery;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateOrderStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Private Helpers

        private void RecalculateAmount()
        {
            var total = orderItemsEntities.Sum(i => i.TotalPrice.Value);

            TotalAmount = new Price(total);
            FinalAmount = new Price(total + ShippingFee - Discount.Value);
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion
    }
}
