namespace auth_service.authservice.application.dtos
{
    public class ResponseAuthenticationByGoogle
    {
        public Guid? Id { get; set; }
        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? AvatarUrl { get; set; }


    }
}
