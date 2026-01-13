using user_service.UserService.Domain.Entities;
using user_service.UserService.Domain.ValueObjects;

namespace user_service.UserService.Domain.Aggregates
{
    public class UserAggregate
    {
        public Guid Id { get; private set; }

        public string? Username { get; private set; }

        public Email Email { get; private set; }

        public PhoneNumber PhoneNumberUser { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime UpdatedAt { get; private set; }

        private List<UserAddresses> _userAddressesStore = new List<UserAddresses>();
        public IReadOnlyCollection<UserAddresses> UserAddresses => _userAddressesStore.AsReadOnly();
        private UserAggregate()
        {

        }

        // Rehydrate method to reconstruct the aggregate from stored data
        public static UserAggregate Rehydrate(Guid id, string username, Email email, PhoneNumber phoneNumber, bool isActive, DateTime createdAt, DateTime updatedAt, List<UserAddresses> userAddresses)
        {
            var aggregate = new UserAggregate
            {
                Id = id,
                Username = username,
                Email = email,
                PhoneNumberUser = phoneNumber,
                IsActive = isActive,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                _userAddressesStore = userAddresses
            };
            return aggregate;
        }




        public static UserAggregate CreateNewUser(string username, Email email, PhoneNumber phoneNumber)
        {
            return new UserAggregate
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PhoneNumberUser = phoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void AddNewAddress(UserAddresses userAddresses)
        {
            _userAddressesStore.Add(userAddresses);
        }

        public void ChangeUserStatus(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }


        public void UpdatePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumberUser = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }


        public void UpdateUsername(string username)
        {
            Username = username;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
