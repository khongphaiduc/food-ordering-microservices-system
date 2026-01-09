namespace auth_services.AuthService.Application.DTOS
{
    public class RequestCreateNewUser
    { 
        public string UserName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;

    }
}
