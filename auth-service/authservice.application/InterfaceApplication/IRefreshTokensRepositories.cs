using auth_service.authservice.application.dtos;

namespace auth_service.authservice.application.InterfaceApplication
{
    public interface IRefreshTokensRepositories
    {
        Task<TokenResult> AddRefreshToken(Guid idUser);

        Task RevokeOldToken(Guid idUser);
        Task<bool> IsInvokedToken (string token);

    }
}
