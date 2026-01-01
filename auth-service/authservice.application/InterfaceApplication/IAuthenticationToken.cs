namespace auth_service.authservice.application.InterfaceApplication
{
    public interface IAuthenticationToken
    {
        public object GenerateToken(string email, string role);

    }
}
