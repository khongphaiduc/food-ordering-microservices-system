using order_service.OrderService.Appilcation.DTOs;

namespace order_service.OrderService.Appilcation.Services
{
    public interface IGetListOrderOfUser
    {
        Task<OrderHistoryPagination> GetListOrderForUser(RequestGetListOrderWithPagination request);
    }
}
