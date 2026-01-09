namespace AuthService.Domain.ValueObject
{
    public class Password
    {
        public string Value { get; set; }

        public Password(string value)
        {
            if (value.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.", nameof(value));
            }

            Value = value;
        }
    }


}
