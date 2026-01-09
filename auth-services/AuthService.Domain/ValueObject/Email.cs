namespace auth_services.AuthService.Domain.ValueObject
{
    public class Email
    {
        public string EmailAdress { get; } = null!;

        public Email(string emailAdress)
        {
            if (string.IsNullOrWhiteSpace(emailAdress) || !emailAdress.Contains(".com"))
            {
                throw new ArgumentException("Email address cannot be null or empty.", nameof(emailAdress));
            }

            EmailAdress = emailAdress;
        }
    }
}
