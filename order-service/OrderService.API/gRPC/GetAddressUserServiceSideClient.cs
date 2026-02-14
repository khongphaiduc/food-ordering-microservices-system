using UserService.API.Protos;

namespace order_service.OrderService.API.gRPC
{
    public class GetAddressUserServiceSideClient
    {
        private readonly UserAddressInfoGrpc.UserAddressInfoGrpcClient _getAddressUser;

        public GetAddressUserServiceSideClient(UserAddressInfoGrpc.UserAddressInfoGrpcClient userAddressInfoGrpcClient)
        {
            _getAddressUser = userAddressInfoGrpcClient;
        }


        public async Task<AddressInformationUserResponse> GetAddressUserAsync(AddressformationUserRequest request)   // id address 
        {
            return await _getAddressUser.GetAddRessUserAsync(request);
        }

    }
}
