using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using payment_service.PaymentService.Application.Services;
using PayOS;
using PayOS.Models.Webhooks;

namespace payment_service.PaymentService.API.PaymentControllers
{
    [Route("api/payments")]
    [ApiController]
    public class PayOSWebhookController : ControllerBase
    {
        private readonly PayOSClient _payOSClient;
        private readonly IUpdateOrderStatus _orderUpdate;
        private readonly ILogger<PayOSWebhookController> _logger;

        public PayOSWebhookController(PayOSClient payOSClient, IUpdateOrderStatus updateOrderStatus, ILogger<PayOSWebhookController> logger)
        {
            _payOSClient = payOSClient;
            _orderUpdate = updateOrderStatus;
            _logger = logger;
        }

        [HttpPost("payos/webhook")]
        public async Task<IActionResult> WebHookAsync([FromBody] Webhook webhook)
        {
            try
            {
                var webhookData = await _payOSClient.Webhooks.VerifyAsync(webhook);

                if (webhookData.Code == "00")
                {
                    await _orderUpdate.Excute(webhookData);
                }
                else
                {
                    _logger.LogError("Payment failed with code: {Code}", webhookData.Code);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PayOS webhook");
            }
            return Ok();
        }
    }
}
