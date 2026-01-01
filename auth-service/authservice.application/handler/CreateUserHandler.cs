using auth_service.authservice.application.dtos;
using auth_service.authservice.application.InterfaceApplication;
using auth_service.authservice.domain.entities;
using auth_service.authservice.domain.Interfaces;

namespace auth_service.authservice.application.handler
{
    public class CreateUserHandler : ICreateUserHandler
    {
        private readonly IUserRepositories _iuserRepo;
        private readonly IHashPassword _iHashPass;

        public CreateUserHandler(IUserRepositories userRepositories, IHashPassword hashPassword)
        {
            _iuserRepo = userRepositories;
            _iHashPass = hashPassword;

        }


        public async Task<RequestAccount> HandleCreateUser(RequestAccount register)
        {

            // check email exists
            if (await _iuserRepo.GetUserByEmail(register.Email)!= null)
            {
                register.IsSuccessful = false;
                register.Message = "Email already exists";
                return new RequestAccount()
                {
                    IsSuccessful = false,
                    Message = "Email already exists"
                };
            }
            var salt = _iHashPass.GenarateSalt();  // hash

            var hashedPassword = _iHashPass.Hash(register.Password, salt); // pass after hash

            var guesuser = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Username = register.Username,
                Email = new domain.value_object.EmailObject(register.Email),
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // domain 
            var result = await _iuserRepo.CreateAccount(guesuser);

            if (result)
            {
                register.IsSuccessful = true;
                register.Message = "Account created successfully";
                return new RequestAccount()
                {
                    IsSuccessful = true,
                    Message = "Account created successfully"
                };
            }
            else
            {
                register.IsSuccessful = false;
                register.Message = "Failed to create account";
                return new RequestAccount()
                {
                    IsSuccessful = false,
                    Message = "Failed to create account"
                };
            }

        }


    }
}
