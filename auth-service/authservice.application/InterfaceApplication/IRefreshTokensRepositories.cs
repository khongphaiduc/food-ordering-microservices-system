using auth_service.authservice.application.dtos;

namespace auth_service.authservice.application.InterfaceApplication
{
    public interface IRefreshTokensRepositories
    {
        Task<TokenResult> AddRefreshToken(Guid idUser);  //tạo mới

        Task RevokeOldToken(Guid idUser); // thu hồi 

        Task<bool> IsInvokedToken (string token);  // check token có bị thu hồi hay không

    }
}
