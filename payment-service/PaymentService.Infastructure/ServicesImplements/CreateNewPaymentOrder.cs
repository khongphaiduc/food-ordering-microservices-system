using OrderService.API.Proto;
using payment_service.PaymentService.API.gRPC;
using payment_service.PaymentService.Application.DTOs;
using payment_service.PaymentService.Application.Services;
using payment_service.PaymentService.Infastructure.Models;
using PayOS;
using PayOS.Models.V2.PaymentRequests;

namespace payment_service.PaymentService.Infastructure.ServicesImplements
{
    public class CreateNewPaymentOrder : ICreateNewPaymentOrder
    {
        private readonly PayOSClient _payOS;
        private readonly GetInfomationOfOrderBygRPC _getOrder;
        private readonly ILogger<CreateNewPaymentOrder> _logger;
        private readonly OrderInforGrpc.OrderInforGrpcClient _orderClient;

        public CreateNewPaymentOrder(GetInfomationOfOrderBygRPC getInfomationOfOrderBygRPC, PayOSClient payOSClient, ILogger<CreateNewPaymentOrder> logger, OrderInforGrpc.OrderInforGrpcClient client)
        {
            _payOS = payOSClient;
            _getOrder = getInfomationOfOrderBygRPC;
            _logger = logger;
            _orderClient = client;
        }

        public async Task<string> Excute(Guid IdOrder, global::OrderService.API.Proto.ResponseOrderInformation order)
        {
            //OrderDTOInternal order = new OrderDTOInternal();

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    //order = await _getOrder.Excute(IdOrder);  // get information of Order through gRPC

                    _logger.LogInformation($"order amount :{order.FinalAmount}");
                    _logger.LogInformation($"order description  :{order.OrderCode}");
                    _logger.LogInformation($"order code :{order.OrderCode}");


                    if (order.OrderID != Guid.Empty.ToString()) break;
                    await Task.Delay(200);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error when get information of order with id {IdOrder}: {Message}", IdOrder, ex.Message);
                    if (i == 2) throw;
                    await Task.Delay(200);
                }
            }


            var paymentRequest = new CreatePaymentLinkRequest
            {
                OrderCode = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                Amount = (long)order.FinalAmount,
                Description = order.OrderCode,
                ReturnUrl = "https://your-url.com",
                CancelUrl = "https://your-url.com"
            };

            var paymentLink = await _payOS.PaymentRequests.CreateAsync(paymentRequest);

            if (paymentLink.QrCode != null)
            {
                return paymentLink.QrCode;
            }
            else
            {
                return "Create Payment Fail";
            }
        }
    }
}
