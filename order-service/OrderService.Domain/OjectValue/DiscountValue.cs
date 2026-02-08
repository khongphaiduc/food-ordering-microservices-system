namespace order_service.OrderService.Domain.OjectValue
{
    public class DiscountValue
    {
        public decimal Value { get; private set; }

        public DiscountValue(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Discount cannot be negative.");
            }
            Value = value;
        }
    }
}
