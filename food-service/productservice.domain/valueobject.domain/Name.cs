namespace food_service.productservice.domain.valueobject.domain
{
    public class Name
    {
        public string Value { get; }

        public Name(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty");

            if (value.Length > 200)
                throw new ArgumentException("Name is too long");

            Value = value.Trim();
        }

    }
}
