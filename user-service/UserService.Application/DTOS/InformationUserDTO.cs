namespace user_service.UserService.Application.DTOS
{
    public class InformationUserDTO
    {
        public Guid Iduser { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public List<AddressUser>? addressUsers { get; set; }
    }

    public class AddressUser
    {
        public Guid IdAddressItem { get; set; }
        public string? Line1 { get; set; }

        public string? Line2 { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }
    }
}
