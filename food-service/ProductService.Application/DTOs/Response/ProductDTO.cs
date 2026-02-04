namespace food_service.ProductService.Application.DTOs.Response
{
    public class ProductDTO
    {
        public Guid IdCategory { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? Decriptions { get; set; }
        public string UrlImageMain { get; set; }

        public bool IsAvailable { get; set; }   // còn bán hay không
    }
}
