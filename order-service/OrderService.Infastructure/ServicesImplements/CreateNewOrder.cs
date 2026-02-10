using order_service.OrderService.API.gRPC;
using order_service.OrderService.Appilcation.Services;
using order_service.OrderService.Domain.Aggregate;
using order_service.OrderService.Domain.Entities;
using order_service.OrderService.Domain.Enums;
using order_service.OrderService.Domain.Interface;

namespace order_service.OrderService.Infastructure.ServicesImplements
{
    public class CreateNewOrder : ICreateNewOrder
    {
        private readonly GetInformationOfCart _cartClientGRPC;
        private readonly IOrderRepository _orderRepository;

        public CreateNewOrder(GetInformationOfCart getInformationOfCartClient, IOrderRepository orderRepository)
        {
            _cartClientGRPC = getInformationOfCartClient;
            _orderRepository = orderRepository;
        }

        public async Task<bool> Excute(Guid IdCart, PaymentMethod paymentMethod)
        {

            var cart = await _cartClientGRPC.Excute(IdCart);  // data cart service 

            if (cart.CartId == Guid.Empty) return false;

            // order
            var newCartAggregate = OrdersAggregate.CreateNewOrder(cart.CartId, cart.UserId, cart.Status, 0, 0, paymentMethod);

            // order items
            if (cart.CartItems != null && cart.CartItems.Any())
            {
                foreach (var item in cart.CartItems)
                {
                    newCartAggregate.AddOrderItem(OrderItemsEntity.CreateOrderItems(newCartAggregate.IdOrder, item.ProductId, item.ProductName, item.VariantId, item.VariantName, (decimal)item.UnitPrice, item.Quantity, (decimal)item.TotalPrice));
                }
            }

            // discount 
            decimal DiscountAmount = 0;

            if (cart.CartDiscounts != null && cart.CartDiscounts.Any())
            {
                DiscountAmount = cart.CartDiscounts.Sum(s => s.DiscountAmount);
                newCartAggregate.SetDiscount(DiscountAmount);
            }

            // payment 
            newCartAggregate.AddOrderPayment(OrderPaymentsEntity.CreateOrderPayment(newCartAggregate.IdOrder, paymentMethod, PaymentStatus.PENDING, newCartAggregate.FinalAmount.Value, null, null));


            return await _orderRepository.CreateNewOrder(newCartAggregate);

        }
    }
}
