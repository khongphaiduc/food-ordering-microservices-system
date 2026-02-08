namespace cart_service.CartService.Application.DTOs
{
    public class ResponseViewCartUser
    {
        public Guid IdCart { get; set; }

        public decimal TotalCart { get; set; }

        public List<CartItems>? cartItems { get; set; }

    }


    public class CartItems
    {
        public Guid CartItemId { get; set; }

        public Guid IdProduct { get; set; }

        public Guid IdVariant { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string? NameProduct { get; set; }

        public string? NameVariant { get; set; }

        public string? UrlImage { get; set; }
    }


}
