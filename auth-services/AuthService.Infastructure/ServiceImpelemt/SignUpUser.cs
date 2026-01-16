using auth_services.AuthService.API.gRPCs;
using auth_services.AuthService.Application.DTOS;
using auth_services.AuthService.Application.Interfaces;
using auth_services.AuthService.Application.Service;
using auth_services.AuthService.Domain.Aggregate;
using auth_services.AuthService.Domain.Interface;
using auth_services.AuthService.Domain.ValueObject;
using auth_services.AuthService.Infastructure.IntegrationContracts;
using auth_services.AuthService.Infastructure.RabbitMQs.Producer;
using UserService.API.Protos;

namespace auth_services.AuthService.Infastructure.ServiceImpelemt
{
    public class SignUpUser : ISignUpUser
    {
        private readonly IGenarateSalt _iGenarateSalt;
        private readonly IHashPassword _iHashPassword;
        private readonly IUserRepository _iUserRepository;
        private readonly RabbitMQProducer _rabbitMQ;
        private readonly IConfiguration _iConfig;
        private readonly UserServicesClient _userClient;

        public SignUpUser(IGenarateSalt genarateSalt, IHashPassword hashPassword, IUserRepository userRepository, RabbitMQProducer rabbitMQProducer, IConfiguration configuration, UserServicesClient userServicesClient)
        {
            _iGenarateSalt = genarateSalt;
            _iHashPassword = hashPassword;
            _iUserRepository = userRepository;
            _rabbitMQ = rabbitMQProducer;
            _iConfig = configuration;
            _userClient = userServicesClient;
        }

        public async Task<bool> Execute(RequestCreateNewUser user)
        {
            if (user.Password != user.ConfirmPassword) return false;

            if (await _iUserRepository.IsExitUser(user.Email)) return false;

            var salt = _iGenarateSalt.GenarateSalt();
            var hashedPassword = _iHashPassword.HandleHashPassword(user.Password, salt);
            // aggregate root
            var userAggregate = UserAggregate.CreateNewUser(new FullNameOfUser(user.UserName), new Email(user.Email), hashedPassword, salt);

            var result = await _iUserRepository.AddNewUser(userAggregate);

            // call gRPC user Client
            await _userClient.CreateNewInformationUserAsync(new CreateNewInformationUserRequest
            {
                Id = userAggregate.Id.ToString(),
                Name = userAggregate.Username.Value,
                Email = userAggregate.Email.EmailAdress, 
                Phone = "0000000000"            // phone mặc dịnhd
            });

            if (result)
            {
                // send message into  rabbbitMQ 
                await _rabbitMQ.SendMessage(new RegisterNotificationMessage
                {
                    Email = userAggregate.Email.EmailAdress,
                    Name = userAggregate.Email.EmailAdress,
                    TypeService = "Email"
                }, _iConfig["RabbitMQ_Side_Auth:Queue:Notification_Email:RoutingKey"]!);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
