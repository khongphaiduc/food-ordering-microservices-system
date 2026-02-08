namespace cart_service.CartService.Domain.ValueObject
{
    public class Quantity
    {
        public int Value { get; private set; }
        public Quantity(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Quantity must be at least 0.");
            }
            Value = value;
        }
    }
}
