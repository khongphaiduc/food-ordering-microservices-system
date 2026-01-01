namespace auth_service.authservice.application.dtos
{
    public class RequestAccount
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
        public string? Message { get; set; }

        public bool IsSuccessful { get; set; }

    }
}
