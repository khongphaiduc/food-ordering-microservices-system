using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using user_service.userservice.infastructure.DBcontextService;
using UserService.API.Protos;

namespace user_service.UserService.API.gRPC
{
    public class AddressUserService : UserAddressInfoGrpc.UserAddressInfoGrpcBase
    {
        private readonly FoodUsersContext _db;

        public AddressUserService(FoodUsersContext foodUsersContext)
        {
            _db = foodUsersContext;
        }

        public override async Task<AddressInformationUserResponse> GetAddRessUser(AddressformationUserRequest request, ServerCallContext context)
        {
            var address = await _db.UserAddresses
        .Include(x => x.User)
        .FirstOrDefaultAsync(s => s.Id == Guid.Parse(request.IdAddress));

            if (address == null)
                return new AddressInformationUserResponse();

            return new AddressInformationUserResponse
            {
                NameUser = address.User?.FullName ?? "Tên Không Xác Định",
                Address = $"{address.AddressLine1}, {address.AddressLine2}, {address.District}, {address.City}",
                Phone = address.User?.PhoneNumber ?? "Không Tìm Thấy Số Điện Thoại Của Người Này",
                Note = address.AddressLine2 ?? "Không Có Ghi Chú",
            };
        }
    }
}
