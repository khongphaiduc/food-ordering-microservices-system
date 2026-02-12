using PayOS.Models.Webhooks;

namespace payment_service.PaymentService.Application.Services
{
    public interface IUpdateOrderStatus
    {
        Task<bool> Excute(WebhookData webhookData);
    }
}
