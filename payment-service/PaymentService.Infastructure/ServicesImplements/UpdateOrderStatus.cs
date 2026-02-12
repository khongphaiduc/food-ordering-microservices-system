using Microsoft.AspNetCore.SignalR;
using OrderService.API.Proto;
using payment_service.PaymentService.Application.Services;
using payment_service.PaymentService.Infastructure.NotificationRealTimeSignalR;
using PayOS.Models.Webhooks;

namespace payment_service.PaymentService.Infastructure.ServicesImplements
{
    public class UpdateOrderStatus : IUpdateOrderStatus
    {
        private readonly OrderServiceUpdateStatusGrpc.OrderServiceUpdateStatusGrpcClient _orderService;
        private readonly ILogger<UpdateOrderStatus> _logger;
        private readonly IHubContext<NotificationPaidSusscessfully> _notificationPayOs;

        public UpdateOrderStatus(OrderServiceUpdateStatusGrpc.OrderServiceUpdateStatusGrpcClient orderServiceUpdateStatusGrpcClient, ILogger<UpdateOrderStatus> logger, IHubContext<NotificationPaidSusscessfully> hubContext)
        {
            _orderService = orderServiceUpdateStatusGrpcClient;
            _logger = logger;
            _notificationPayOs = hubContext;
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
                _logger.LogInformation($"UserID is : {result.UserID}");
                _logger.LogInformation($"SendMessage");
                await _notificationPayOs.Clients.User(result.UserID).SendAsync("mynofication", $"Bạn đã thanh toán thành công {webhookData.Amount}");
                _logger.LogInformation($"End SendMessage");
            }
            else
            {
                _logger.LogError(result.MessageUpdate);
                await _notificationPayOs.Clients.User(result.UserID).SendAsync("mynofication", $"Thanh toán không thành công {webhookData.Amount}");
            }
            return result.StatusUpdateOrder;
        }
    }
}
