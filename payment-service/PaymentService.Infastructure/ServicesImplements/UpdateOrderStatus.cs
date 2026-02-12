using OrderService.API.Proto;
using payment_service.PaymentService.Application.Services;
using PayOS.Models.Webhooks;

namespace payment_service.PaymentService.Infastructure.ServicesImplements
{
    public class UpdateOrderStatus : IUpdateOrderStatus
    {
        private readonly OrderServiceUpdateStatusGrpc.OrderServiceUpdateStatusGrpcClient _orderService;
        private readonly ILogger<UpdateOrderStatus> _logger;

        public UpdateOrderStatus(OrderServiceUpdateStatusGrpc.OrderServiceUpdateStatusGrpcClient orderServiceUpdateStatusGrpcClient, ILogger<UpdateOrderStatus> logger)
        {
            _orderService = orderServiceUpdateStatusGrpcClient;
            _logger = logger;
        }

        public async Task<bool> Excute(WebhookData webhookData)
        {
            _logger.LogInformation($"Order code is : {webhookData.OrderCode}");
            var result = await _orderService.UpdateStatusOrderAsync(new RequestOrderUpdateStatus
            {
                OrderCode = webhookData.OrderCode.ToString(),
            });
            if (result.StatusUpdateOrder)
            {
                _logger.LogInformation(result.MessageUpdate);
            }
            else
            {
                _logger.LogError(result.MessageUpdate);
            }
            return result.StatusUpdateOrder;
        }
    }
}
