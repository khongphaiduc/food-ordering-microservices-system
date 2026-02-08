using cart_service.CartService.Domain.ValueObject;

namespace cart_service.CartService.Domain.Entities
{
    public class CartDiscountEntity
    {
        public Guid Id { get; private set; }

        public Guid CartId { get; private set; }


        public string Code { get; private set; } = null!;

        public string DiscountType { get; private set; } = null!;

        public PercentDiscount DiscountValue { get; private set; }

        public Price AppliedAmount { get; private set; }


        public CartDiscountEntity CreateNewCartDiscount(Guid CartId, string Code, string DiscountType, decimal DiscountValue, decimal AppliedAmount)
        {
            return new CartDiscountEntity
            {
                Id = Guid.NewGuid(),
                CartId = CartId,
                Code = Code,
                DiscountType = DiscountType,
                DiscountValue = new PercentDiscount(DiscountValue),
                AppliedAmount = new Price(AppliedAmount)
            };
        }

        internal CartDiscountEntity(Guid id, Guid cartId, string code, string discountType, PercentDiscount discountValue, Price appliedAmount)
        {
            Id = id;
            CartId = cartId;
            Code = code;
            DiscountType = discountType;
            DiscountValue = discountValue;
            AppliedAmount = appliedAmount;
        }

        public CartDiscountEntity()
        {
        }

    }
}
