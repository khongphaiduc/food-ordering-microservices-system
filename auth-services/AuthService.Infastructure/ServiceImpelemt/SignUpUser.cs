using auth_services.AuthService.Application.DTOS;
using auth_services.AuthService.Application.Interfaces;
using auth_services.AuthService.Application.Service;
using auth_services.AuthService.Domain.Aggregate;
using auth_services.AuthService.Domain.Interface;
using auth_services.AuthService.Domain.ValueObject;

namespace auth_services.AuthService.Infastructure.ServiceImpelemt
{
    public class SignUpUser : ISignUpUser
    {
        private readonly IGenarateSalt _iGenarateSalt;
        private readonly IHashPassword _iHashPassword;
        private readonly IUserRepository _iUserRepository;

        public SignUpUser(IGenarateSalt genarateSalt, IHashPassword hashPassword, IUserRepository userRepository)
        {
            _iGenarateSalt = genarateSalt;
            _iHashPassword = hashPassword;
            _iUserRepository = userRepository;
        }

        public async Task<bool> Execute(RequestCreateNewUser user)
        {
            if (user.Password != user.ConfirmPassword) return false;

            if (await _iUserRepository.IsExitUser(user.Email)) return false;

            var salt = _iGenarateSalt.GenarateSalt();
            var hashedPassword = _iHashPassword.HandleHashPassword(user.Password, salt);
            // aggregate root
            var userAggregate = UserAggregate.CreateNewUser(new FullNameOfUser(user.UserName), new Email(user.Email), hashedPassword, salt);
            return await _iUserRepository.AddNewUser(userAggregate);  // thêm user

        }
    }
}
