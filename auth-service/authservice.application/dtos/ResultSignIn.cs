namespace auth_service.authservice.application.dtos
{
    public class ResultSignIn
    {
        public string Name { get; set; } = null!;

        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;

        public DateTime RefreshTokenExpiry { get; set; }

        public string? Message { get; set; }
    }
}
