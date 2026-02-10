namespace payment_service.PaymentService.Application.Services
{
    public interface ICreateNewPaymentOrder
    {
        Task<string> Excute(Guid IdOrder, global::OrderService.API.Proto.ResponseOrderInformation order);
    }

}
