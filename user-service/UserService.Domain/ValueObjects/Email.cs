namespace user_service.UserService.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            {
                throw new ArgumentException("Invalid email address.", nameof(value));
            }
            Value = value;
        }
    }
}
