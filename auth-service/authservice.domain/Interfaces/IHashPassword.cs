namespace auth_service.authservice.domain.Interfaces
{
    public interface IHashPassword
    {
        string GenarateSalt();
        string Hash(string password, string salt);
    }
}
