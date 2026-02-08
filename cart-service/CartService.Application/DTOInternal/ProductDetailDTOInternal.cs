namespace cart_service.CartService.Application.DTOInternal
{
    public class ProductDetailDTOInternal
    {
        public Guid IdCategory { get; set; }

        public Guid IdProduct { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Desciption { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string StatusCode { get; set; } = string.Empty;
        public List<ProductImageDTOInternal>? ListImage { get; set; }

        public List<ProductVariantDTOInternal>? ListVariant { get; set; }

    }
}
