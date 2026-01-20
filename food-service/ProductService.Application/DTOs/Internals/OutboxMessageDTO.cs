namespace food_service.ProductService.Application.DTOs.Internals
{
    public class OutboxMessageDTO
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string PayLoad { get; set; }

        public bool IsProcesced { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
