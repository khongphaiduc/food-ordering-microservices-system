namespace auth_services.AuthService.Domain.Entities
{
    public class RefreshTokenEntity
    {
        public Guid Id { get; private set; }

        public Guid Token { get; private set; }

        public DateTime ExpireAt { get; private set; }

        public DateTime CreateAt { get; private set; }

        public string Device { get; private set; } 
        private RefreshTokenEntity()
        {
        }

        public static RefreshTokenEntity CreateNewRefreshToken(Guid Token, DateTime ExpireAt)
        {
            return new RefreshTokenEntity()
            {
                Id = Guid.NewGuid(),
                Token = Token,
                ExpireAt = ExpireAt,
                CreateAt = DateTime.UtcNow,
                Device = "Website"
            };
        }

    }
}
