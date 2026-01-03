namespace auth_service.authservice.application.dtos
{
    public class LogOutRequest
    {
        public Guid UserId { get; set; }

        public string? Message { get; set; }

    }
}
