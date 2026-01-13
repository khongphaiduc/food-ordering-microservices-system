namespace user_service.UserService.Application.DTOS
{
    public class RequestUserProfile
    {

        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }


    }
}
