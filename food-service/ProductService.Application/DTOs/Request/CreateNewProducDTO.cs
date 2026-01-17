namespace food_service.ProductService.Application.DTOs.Request
{
    public class CreateNewProducDTO
    {
        public Guid IdCategory { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

    }
}
