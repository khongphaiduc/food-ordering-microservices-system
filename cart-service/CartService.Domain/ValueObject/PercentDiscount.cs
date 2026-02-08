namespace cart_service.CartService.Domain.ValueObject
{
    public class PercentDiscount
    {
        public decimal Value { get; private set; }
        public PercentDiscount(decimal discountValue)
        {
            if (discountValue < 0 || discountValue > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(discountValue), "Discount value must be between 0 and 100.");
            }
            Value = discountValue;
        }
    }
}
