using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using order_service.OrderService.Domain.Enums;
using order_service.OrderService.Infastructure.Models;
using OrderService.API.Proto;

namespace order_service.OrderService.API.gRPC
{
    public class GetOrderSubServices : OrderInforGrpc.OrderInforGrpcBase
    {
        private readonly FoodOrderContext _db;

        public GetOrderSubServices(FoodOrderContext foodOrderContext)
        {
            _db = foodOrderContext;
        }

        public override async Task<ResponseOrderInformation> GetInformationOrder(RequestOrder request, ServerCallContext context)
        {
            var Order = await _db.Orders.Where(x => x.Id == Guid.Parse(request.OrderID) && x.Status == OrderStatus.PENDING.ToString()).FirstOrDefaultAsync();

            if (Order == null)
            {
                return new ResponseOrderInformation
                {
                    OrderID = Guid.Empty.ToString(),
                    FinalAmount = 0,
                };
            }
            else
            {
                return new ResponseOrderInformation
                {
                    OrderID = Order.Id.ToString(),
                    FinalAmount = (long)Order.FinalAmount,
                    OrderCode = Order.OrderCode,
                    OrderStatus = Order.Status,
                    UserID = Order.UserId.ToString(),
                };

            }
        }
    }
}
