using OrderService.API.Proto;
using payment_service.PaymentService.Application.DTOs;
using payment_service.PaymentService.Domain.Enums;

namespace payment_service.PaymentService.API.gRPC
{
    public class GetInfomationOfOrderBygRPC
    {
        private readonly OrderInforGrpc.OrderInforGrpcClient _orderClient;

        public GetInfomationOfOrderBygRPC(OrderInforGrpc.OrderInforGrpcClient orderInforGrpcClient)
        {
            _orderClient = orderInforGrpcClient;
        }

        public async Task<OrderDTOInternal> Excute(Guid IdOrder)
        {
            var order = await _orderClient.GetInformationOrderAsync(new RequestOrder { OrderID = IdOrder.ToString() },deadline:DateTime.UtcNow.AddSeconds(4));

            if (order.OrderID == Guid.Empty.ToString()) return new OrderDTOInternal { OrderId = Guid.Empty };
            return new OrderDTOInternal
            {
                OrderId = Guid.Parse(order.OrderID),
                UserId = Guid.Parse(order.UserID),
                TotalAmount = (decimal)order.FinalAmount,
                Status = Enum.Parse<OrderStatus>(order.OrderStatus),
                OrderCode = order.OrderCode
            };
        }



    }
}
