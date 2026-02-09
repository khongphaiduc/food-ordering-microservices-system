using order_service.OrderService.API.gRPC;
using order_service.OrderService.Appilcation.Services;

namespace order_service.OrderService.Infastructure.ServicesImplements
{
    public class CreateNewOrder : ICreateNewOrder
    {
        private readonly GetInformationOfCart _cartClientGRPC;

        public CreateNewOrder(GetInformationOfCart getInformationOfCartClient)
        {
            _cartClientGRPC = getInformationOfCartClient;
        }

        public async Task<bool> Excute(Guid IdCart)
        {

            var cart = await _cartClientGRPC.Excute(IdCart);

            if (cart.CartId == Guid.Empty) return false;

            // continuous 10/2/2025




            //
            return true;
        }
    }
}
