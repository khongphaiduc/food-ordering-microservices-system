namespace food_service.ProductService.Application.Service
{
    public interface IUpdateProduct
    {
        Task Excute(Guid IdProduct);
    }
}
