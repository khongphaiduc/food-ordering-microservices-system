namespace auth_services.AuthService.Infastructure.IntegrationContracts
{
    public class RegisterNotificationMessage
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public string TypeService { get; set; } = "Email";

    }
}
