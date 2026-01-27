namespace food_service.ProductService.Application.Interface
{
    public interface IMinIOFood
    {
        Task<string> UploadAsync(IFormFile file);
        Task DeleteAsync(string objectName);
    }
}
