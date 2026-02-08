using cart_service.CartService.Domain.ValueObject;

namespace cart_service.CartService.Domain.Entities
{
    public class CartItemEntity
    {
        public Guid Id { get; private set; }

        public Guid CartId { get; private set; }

        public Guid ProductId { get; private set; }

        public Guid? VariantId { get; private set; }


        public string ProductName { get; private set; } = null!;

        public string? VariantName { get; private set; }

        public Price UnitPrice { get; private set; } = new Price(0);

        public Quantity Quantitys { get; private set; } = null!;

        public Price TotalPrice { get; private set; } = null!;

        public DateTime CreatedAt { get; private set; }


        public DateTime UpdatedAt { get; private set; }

        public static CartItemEntity CreateNewCartItem(Guid CartId, Guid ProductId, Guid? VariantID, string? VariantName, string ProductName, decimal UnitPrice, int quantity)
        {
            return new CartItemEntity
            {
                Id = Guid.NewGuid(),
                CartId = CartId,
                ProductId = ProductId,
                VariantId = VariantID,
                VariantName = VariantName,
                ProductName = ProductName,
                UnitPrice = new Price(UnitPrice),
                Quantitys = new Quantity(quantity),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                TotalPrice = new Price(UnitPrice * quantity),
            };
        }



        internal CartItemEntity(Guid id, Guid cartId, Guid productId, Guid? variantId, string productName, string? variantName, Price unitPrice, Quantity quantitys, Price totalPrice, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            CartId = cartId;
            ProductId = productId;
            VariantId = variantId;
            ProductName = productName;
            VariantName = variantName;
            UnitPrice = unitPrice;
            Quantitys = quantitys;
            TotalPrice = totalPrice;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public CartItemEntity()
        {
        }

        public void ChangeName(string name)
        {
            if (VariantId == null) throw new InvalidOperationException("Cannot change variant name for product root");
            VariantName = name;
            UpdatedAt = DateTime.UtcNow;
        }



        public void ChangeQuantity(int quantity)
        {
            Quantitys = new Quantity(quantity);
            TotalPrice = new Price(UnitPrice.Value * quantity);
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
