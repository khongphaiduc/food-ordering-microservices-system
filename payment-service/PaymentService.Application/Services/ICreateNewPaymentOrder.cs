using payment_service.PaymentService.Application.DTOs;

namespace payment_service.PaymentService.Application.Services
{
    public interface ICreateNewPaymentOrder
    {
        Task<string> Excute(OrderDTOInternal order);
    }

}
