namespace user_service.UserService.Domain.Entities
{
    public class UserAddresses
    {
        public Guid Id { get; private set; }

        public Guid IdUser { get; private set; }

        public string AddressLine1 { get; private set; }

        public string? AddressLine2 { get; private set; }

        public string City { get; private set; }

        public string District { get; private set; }

        public string PostalCode { get; private set; }

        public bool IsDefault { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime UpdatedAt { get; private set; }

        public UserAddresses()
        {
        }

        public static UserAddresses CreateNewAddress(Guid idUser, string addressLine1, string? addressLine2, string city, string district, string postalCode, bool isDefault)
        {
            return new UserAddresses
            {
                Id = Guid.NewGuid(),
                IdUser = idUser,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                District = district,
                PostalCode = postalCode,
                IsDefault = isDefault,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        internal UserAddresses(Guid id, Guid idUser, string addressLine1, string? addressLine2, string city, string district, string postalCode, bool isDefault, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            IdUser = idUser;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            District = district;
            PostalCode = postalCode;
            IsDefault = isDefault;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public void ChangeDefaultStatus(bool isDefault)
        {
            IsDefault = isDefault;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAddress(string addressLine1, string? addressLine2, string city, string district, string postalCode)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            District = district;
            PostalCode = postalCode;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
