namespace auth_service.authservice.domain.value_object
{
    public class FullNameOfUser
    {
        public string Value { get; set; }

        public FullNameOfUser(string value)
        {

            if(string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Full name cannot be empty.");
            }

            Value = value;
        }
    }
}
