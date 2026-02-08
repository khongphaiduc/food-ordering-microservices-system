namespace cart_service.CartService.Domain.ValueObject
{
    public class Price
    {
        public decimal Value { get; private set; }

        public Price(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }
            Value = price;
        }

    }
}
