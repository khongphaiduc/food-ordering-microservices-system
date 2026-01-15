using Grpc.Core;
using user_service.UserService.Application.DTOS;
using user_service.UserService.Application.Services;
using UserService.API.Protos;

namespace user_service.UserService.API.gRPC
{
    public class UserInfoSerivce : UserInfoGrpc.UserInfoGrpcBase
    {
        private readonly IUserProfile _iUserRrofile;

        public UserInfoSerivce(IUserProfile userProfile)
        {
            _iUserRrofile = userProfile;
        }

        public override async Task<CreateNewInformationUserResponse> CreateNewInformationUser(CreateNewInformationUserRequest request, ServerCallContext context)
        {

            RequestUserProfile userProfile = new RequestUserProfile
            {
                Id = Guid.Parse(request.Id),
                FullName = request.Name,
                Email = request.Email,
                PhoneNumber = request.Phone
            };

            var result = await _iUserRrofile.UserProfilHandle(userProfile);

            if (result)
            {
                return new CreateNewInformationUserResponse
                {
                    IsSuccess = true,

                };
            }
            else
            {
                return new CreateNewInformationUserResponse
                {
                    IsSuccess = false,
                };
            }
        }

    }
}