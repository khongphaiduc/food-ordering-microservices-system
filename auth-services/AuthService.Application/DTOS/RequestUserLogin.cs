namespace auth_services.AuthService.Application.DTOS
{
    public class RequestUserLogin
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
