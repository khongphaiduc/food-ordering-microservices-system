namespace user_service.UserService.Application.DTOS
{
    public class RequestCreateNewAddressUser
    {
        public Guid IdUser { get; set; }

        public string Phone { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Line1 { get; set; } = null!;

        public string? Line2 { get; set; }

        public string? District { get; set; }

        public bool IsDefault { get; set; } 
    }
}
