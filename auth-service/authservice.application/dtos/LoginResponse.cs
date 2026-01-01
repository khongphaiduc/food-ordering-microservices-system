namespace auth_service.authservice.application.dtos
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public Guid? UserId { get; set; } 
        public string Message { get; set; }
    }
}
