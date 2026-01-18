namespace food_service.ProductService.Application.DTOs.Request
{
    public class UpdateCategoryDTO
    {
        public Guid Id { get; set; }  
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}
