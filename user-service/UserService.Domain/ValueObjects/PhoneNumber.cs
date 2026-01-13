namespace user_service.UserService.Domain.ValueObjects
{
    public record PhoneNumber
    {
        public string value { get; init; }
        public PhoneNumber(string number)
        {
            // Basic validation for phone number format (you can enhance this as needed)
            if (string.IsNullOrWhiteSpace(number) || number.Length < 7 || number.Length > 15)
            {
                throw new ArgumentException("Invalid phone number format.", nameof(number));
            }
            value = number;
        }
    }
}
