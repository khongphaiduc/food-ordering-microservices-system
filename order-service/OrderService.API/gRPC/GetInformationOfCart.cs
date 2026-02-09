using CartService.API.Protos;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using order_service.OrderService.Appilcation.DTOs.DTOsInternal;
using System.Threading.Tasks;

namespace order_service.OrderService.API.gRPC
{
    public class GetInformationOfCart
    {
        private readonly CartInforGrpc.CartInforGrpcClient _cartInforGrpcClient;

        public GetInformationOfCart(CartInforGrpc.CartInforGrpcClient cartInforGrpcClient)
        {
            _cartInforGrpcClient = cartInforGrpcClient;
        }

        #region get information of cart
        public async Task<CartDTOsInternal> Excute(Guid IdCart)
        {
            var cart = await _cartInforGrpcClient.GetInformationCartAsync(new CartID { IdCart = IdCart.ToString() });
            if (cart == null || cart.IdCart == Guid.Empty.ToString())
            {
                return new CartDTOsInternal
                {
                    CartId = Guid.Empty,
                    CartItems = new List<CartItemDTOsInternal>(),
                    CartDiscounts = new List<CartDiscountDTOsInternal>(),
                    TotalPrice = 0
                };
            }
            return new CartDTOsInternal
            {

                CartId = Guid.Parse(cart.IdCart),
                UserId = Guid.Parse(cart.UserId),
                Status = cart.Status,
                TotalPrice = cart.TotalPrice,
                CartItems = cart.CartItems.Select(x => new CartItemDTOsInternal
                {
                    CartId = Guid.Parse(x.CartId),
                    ProductId = Guid.Parse(x.ProductId),
                    ProductName = x.ProductName,
                    VariantId = string.IsNullOrEmpty(x.VariantId) ? null : Guid.Parse(x.VariantId),
                    VariantName = x.VariantName ?? null,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice
                }).ToList(),
                CartDiscounts = cart.CartDiscounts.Select(x => new CartDiscountDTOsInternal
                {
                    CartId = Guid.Parse(x.CartId),
                    DiscountCode = x.DiscountCode,
                    DiscountAmount = x.DiscountAmount,
                    TotalDiscountAmount = x.AmountAfterDiscount
                }).ToList()

            };
        }
        #endregion

    }
}
