using cart_service.CartService.Infastructure.Models;
using CartService.API.Protos;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace cart_service.CartService.API.gRPC
{
    public class CartInforService : CartInforGrpc.CartInforGrpcBase
    {
        private readonly FoodProductsDbContext _db;

        public CartInforService(FoodProductsDbContext foodProductsDbContext)
        {
            _db = foodProductsDbContext;
        }
        #region Get information cart
        public override async Task<global::CartService.API.Protos.Cart> GetInformationCart(CartID request, ServerCallContext context)
        {
            var cart = await _db.Carts.Include(s => s.CartItems).Include(s => s.CartDiscounts).FirstOrDefaultAsync(c => c.Id == Guid.Parse(request.IdCart));
            if (cart == null) return new global::CartService.API.Protos.Cart { IdCart = Guid.Empty.ToString() };
            var cartProto = new global::CartService.API.Protos.Cart
            {
                IdCart = cart.Id.ToString(),
                UserId = cart.UserId.ToString(),
                TotalPrice = (long)cart.TotalPrice,
                Status = cart.Status,
                CartItems =
                {
                    cart.CartItems.Select(s =>  new global::CartService.API.Protos.CartItem
                    {
                        CartId = s.CartId.ToString(),
                        ProductId = s.ProductId.ToString(),
                        ProductName = s.ProductName,
                        VariantId = s.VariantId?.ToString() ?? "",
                        VariantName = s.VariantName ?? "",
                        Quantity = s.Quantity,
                        UnitPrice = (long)s.UnitPrice,
                        TotalPrice = (long)s.TotalPrice
                    })
                },
                CartDiscounts =
                {
                    cart.CartDiscounts.Select(s => new global::CartService.API.Protos.CartDiscount
                    {
                        CartId = s.CartId.ToString(),
                        DiscountCode = s.Code,
                        DiscountAmount = (long)s.DiscountValue,
                        AmountAfterDiscount = (long)s.AppliedAmount,
                    })
                 }
            };
            return cartProto;
        }
        #endregion
    }
}
