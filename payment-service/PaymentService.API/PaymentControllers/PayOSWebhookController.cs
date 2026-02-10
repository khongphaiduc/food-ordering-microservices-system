using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayOS;
using PayOS.Models.Webhooks;

namespace payment_service.PaymentService.API.PaymentControllers
{
    [Route("api/payment/payos")]
    [ApiController]
    public class PayOSWebhookController : ControllerBase
    {
        private readonly PayOSClient _payOSClient;

        public PayOSWebhookController(PayOSClient payOSClient)
        {
            _payOSClient = payOSClient;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHookAsync([FromBody] Webhook webhook)
        {
            var webhookData = await _payOSClient.Webhooks.VerifyAsync(webhook);

            if (webhookData.Code == "00")
            {

            }
            else
            {

            }

            return Ok();
        }
    }
}
