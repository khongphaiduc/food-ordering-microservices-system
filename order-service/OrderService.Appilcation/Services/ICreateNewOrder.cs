namespace order_service.OrderService.Appilcation.Services
{
    public interface ICreateNewOrder
    {
        Task<bool> Excute(Guid IdCart);
    }
}
