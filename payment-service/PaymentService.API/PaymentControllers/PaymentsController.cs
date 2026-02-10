using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.Proto;
using payment_service.PaymentService.Application.DTOs;
using payment_service.PaymentService.Application.Services;
using System.Threading.Tasks;

namespace payment_service.PaymentService.API.PaymentControllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ICreateNewPaymentOrder _createPayment;
        private readonly OrderInforGrpc.OrderInforGrpcClient _client;

        public PaymentsController(ICreateNewPaymentOrder createNewPaymentOrder, OrderInforGrpc.OrderInforGrpcClient client)
        {
            _createPayment = createNewPaymentOrder;
            _client = client;
        }

        //[HttpPost("payos")]
        //public async Task<ActionResult> CreatePaymentOrder([FromBody] RequestCreatePaymentOrder request)
        ////{
        ////    var QRCode = await _createPayment.Excute(request.IdOrder);
        //    return Ok(QRCode);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> actionResult([FromRoute] Guid id)
        {
            var result = await _client.GetInformationOrderAsync(new RequestOrder { OrderID = id.ToString() }, deadline: DateTime.UtcNow.AddSeconds(4));
            var QRCode = await _createPayment.Excute(id, result);
            return Ok(QRCode);
        }
    }
}
