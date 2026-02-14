using order_service.OrderService.API.gRPC;
using order_service.OrderService.Appilcation.DTOs;
using order_service.OrderService.Appilcation.Services;
using order_service.OrderService.Domain.Aggregate;
using order_service.OrderService.Domain.Entities;
using order_service.OrderService.Domain.Enums;
using order_service.OrderService.Domain.Interface;
using PaymentService.API.Proto;

namespace order_service.OrderService.Infastructure.ServicesImplements
{
    public class CreateNewOrder : ICreateNewOrder
    {
        private readonly GetInformationOfCart _cartClientGRPC;
        private readonly IOrderRepository _orderRepository;
        private readonly PaymentInforGrpc.PaymentInforGrpcClient _createPaymentPayOs;
        private readonly ILogger<CreateNewOrder> _logger;

        public CreateNewOrder(GetInformationOfCart getInformationOfCartClient, IOrderRepository orderRepository, PaymentInforGrpc.PaymentInforGrpcClient paymentInforGrpcClient, ILogger<CreateNewOrder> logger)
        {
            _cartClientGRPC = getInformationOfCartClient;
            _orderRepository = orderRepository;
            _createPaymentPayOs = paymentInforGrpcClient;
            _logger = logger;
        }

        public async Task<string> Excute(Guid IdCart, PaymentMethod paymentMethod)
        {

            var cart = await _cartClientGRPC.Excute(IdCart);  // data cart service 

            if (cart.CartId == Guid.Empty) return string.Empty;

            // order
            var newOrderAggregate = OrdersAggregate.CreateNewOrder(cart.CartId, cart.UserId, cart.Status, 0, 0, paymentMethod);

            // order items
            if (cart.CartItems != null && cart.CartItems.Any())
            {
                foreach (var item in cart.CartItems)
                {
                    newOrderAggregate.AddOrderItem(OrderItemsEntity.CreateOrderItems(newOrderAggregate.IdOrder, item.ProductId, item.ProductName, item.VariantId, item.VariantName, (decimal)item.UnitPrice, item.Quantity, (decimal)item.TotalPrice));
                }
            }

            // discount 
            decimal DiscountAmount = 0;

            if (cart.CartDiscounts != null && cart.CartDiscounts.Any())
            {
                DiscountAmount = cart.CartDiscounts.Sum(s => s.DiscountAmount);
                newOrderAggregate.SetDiscount(DiscountAmount);
            }

            // payment 
            newOrderAggregate.AddOrderPayment(OrderPaymentsEntity.CreateOrderPayment(newOrderAggregate.IdOrder, paymentMethod, PaymentStatus.PENDING, newOrderAggregate.FinalAmount.Value, null, null));

            var resultCreateNewOrder = await _orderRepository.CreateNewOrder(newOrderAggregate);  //   tạo order

            if (resultCreateNewOrder.Status)
            {
                var resultChangeStatusCart = await _cartClientGRPC.ChangeStatusCart(cart.CartId, StatusCart.CHECKED_OUT);  //  change status cart = Checked out

                if (resultChangeStatusCart == false) return string.Empty;


                // thanh toán tiền mặt
                if (paymentMethod != PaymentMethod.PayOS)
                {
                    return "Success";
                }


                // create url payment 
                var QRCodeString = await _createPaymentPayOs.CreateNewPaymentAsync(new global::PaymentService.API.Proto.RequestOrderCreatePayment
                {
                    OrderId = resultCreateNewOrder.IdOrder.ToString(),
                    DiscountAmount = 0,
                    FinalAmount = (double)resultCreateNewOrder.FinalAmount,
                    OrderCode = resultCreateNewOrder.OrderCode,
                });

                if (QRCodeString.StatusCreatePayment == "Success")
                {
                    return QRCodeString.QRCodeString;
                }
            }


            return string.Empty;
        }
    }
}
