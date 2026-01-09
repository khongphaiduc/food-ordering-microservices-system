using auth_service.authservice.application.dtos;
using auth_service.authservice.application.InterfaceApplication;
using auth_service.authservice.domain.entities;
using auth_service.authservice.domain.Interfaces;
using auth_service.authservice.domain.value_object;


namespace auth_service.authservice.application.handler
{
    public class UserSignInHandler : IUserLogin
    {
        private readonly IHashPassword _ihasspass;
        private readonly IUserRepositories _iUserRepo;

        public UserSignInHandler(IHashPassword hashPassword, IUserRepositories repositories)
        {
            _ihasspass = hashPassword;
            _iUserRepo = repositories;
        }

        public async Task<LoginResponse> LoginHandler(RequestAccount request)
        {
            var userEmail = new Email(request.Email);

            var userRequest = new UserEntity(userEmail, request.Password);

            var user = await _iUserRepo.GetUserByEmail(userRequest.Email.EmailAdress);

            if (user == null)   // kiểm tra email có tồn tại trong db ko 
            {
                return new LoginResponse() { IsSuccess = false, Message = "Invalid email or password." };
            }


            var salt = await _iUserRepo.GetSalt(userRequest.Email.EmailAdress);     // lấy salt 

            var hashpass = _ihasspass.Hash(userRequest.Password, salt);  // hash với pass thằng user gửi lên 

            var passwordHashFromDb = await _iUserRepo.GetPasswordHash(userRequest.Email.EmailAdress);  // mã has trong db 

            if (hashpass == passwordHashFromDb)
            {
                return new LoginResponse() { IsSuccess = true, UserId = user.Id };
            }
            else
            {
                return new LoginResponse() { IsSuccess = false, Message = "Invalid email or password." };
            }
        }

    }
}
