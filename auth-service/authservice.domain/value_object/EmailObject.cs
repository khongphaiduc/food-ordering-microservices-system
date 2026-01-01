namespace auth_service.authservice.domain.value_object
{
    public class EmailObject
    {
        public string EmailAdress { get; } = null!;

        public EmailObject(string emailAdress)
        {
            if (string.IsNullOrWhiteSpace(emailAdress) || !emailAdress.Contains(".com"))
            {
                throw new ArgumentException("Email address cannot be null or empty.", nameof(emailAdress));
            }

            EmailAdress = emailAdress;
        }
    }
}
