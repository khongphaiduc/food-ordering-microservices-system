namespace food_service.productservice.domain.valueobject.domain
{
    public record Price
    {

        public decimal Amount { get; init; }

        public Price(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Price cannot be negative", nameof(amount));
            }
            Amount = amount;
        }

    }
}
