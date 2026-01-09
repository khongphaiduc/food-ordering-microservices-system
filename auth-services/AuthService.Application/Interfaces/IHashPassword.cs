namespace auth_services.AuthService.Application.Interfaces
{
    public interface IHashPassword
    {
        string HandleHashPassword(string password , string salt);
    }
}
