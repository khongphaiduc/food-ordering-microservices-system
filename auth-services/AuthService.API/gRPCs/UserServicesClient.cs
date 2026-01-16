using UserService.API.Protos;

namespace auth_services.AuthService.API.gRPCs
{
    public class UserServicesClient
    {
        private UserInfoGrpc.UserInfoGrpcClient _userClient;

        public UserServicesClient(UserInfoGrpc.UserInfoGrpcClient userInfoGrpcClient)
        {
            _userClient = userInfoGrpcClient;
        }

        public async Task<CreateNewInformationUserResponse> CreateNewInformationUserAsync(CreateNewInformationUserRequest request)
        {
            return await _userClient.CreateNewInformationUserAsync(request);
        }

    }
}
