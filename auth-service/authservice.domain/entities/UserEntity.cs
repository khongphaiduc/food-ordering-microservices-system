using auth_service.authservice.domain.value_object;

namespace auth_service.authservice.domain.entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public Email Email { get; set; } = null!;

        public string Password { get; set; } = null!; // pass gốc của user gửi lên 

        public string PasswordHash { get; set; } = null!;

        public string PasswordSalt { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


        public UserEntity(Email email, string password)
        {

            if (password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.", nameof(password));
            }


            Email = email;
            Password = password;
        }

        public UserEntity()
        {
        }
    }
}
