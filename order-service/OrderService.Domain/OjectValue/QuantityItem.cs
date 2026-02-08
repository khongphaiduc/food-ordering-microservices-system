namespace order_service.OrderService.Domain.OjectValue
{
    public class QuantityItem
    {
        public int Value { get; private set; }

        public QuantityItem(int value)
        {
            if (value <=1)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            Value = value;
        }
    }
}
