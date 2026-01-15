using UserService.API.Protos;

namespace auth_services.AuthService.API.gRPCs
{
    public class UserServices
    {
        private UserInfoGrpc.UserInfoGrpcClient _userClient;

        public UserServices(UserInfoGrpc.UserInfoGrpcClient userInfoGrpcClient)
        {
            _userClient = userInfoGrpcClient;
        }



    }
}
