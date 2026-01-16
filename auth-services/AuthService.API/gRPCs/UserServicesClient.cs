using Grpc.Core;
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
            return await _userClient.CreateNewInformationUserAsync(request, deadline: DateTime.UtcNow.AddSeconds(3));  // set dealine 
        }
        // nếu vựa quá thời gian mà server chưa trả lời thì client sẽ tự động hủy gRPC và Ném Exception 
        /*        RpcException
                  StatusCode = DeadlineExceeded
                  Message = "Deadline Exceeded"
         */
    }
}
