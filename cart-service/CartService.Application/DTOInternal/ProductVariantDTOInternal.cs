namespace cart_service.CartService.Application.DTOInternal
{
    public class ProductVariantDTOInternal
    {
        public Guid IdVariant { get; set; }

        public string Name { get; set; } = null!;

        public decimal ExtraPrice { get; set; }

        public string TypeVariant { get; set; } = null!;

    }
}
