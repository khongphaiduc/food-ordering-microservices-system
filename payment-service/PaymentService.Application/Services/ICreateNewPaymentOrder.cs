namespace payment_service.PaymentService.Application.Services
{
    public interface ICreateNewPaymentOrder
    {
        Task<string> Excute(Guid IdOrder);
    }

}
