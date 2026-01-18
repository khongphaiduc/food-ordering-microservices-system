namespace search_service.SearchService.Application.IndexModel
{
    public class ProductDoc
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public Guid CategoryID { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public List<ImageDoc> imagesProduct { get; set; }
    }
}
