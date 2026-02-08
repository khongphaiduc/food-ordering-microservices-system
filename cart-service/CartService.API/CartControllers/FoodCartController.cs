using cart_service.CartService.API.gRPC;
using cart_service.CartService.Application.DTOs;
using cart_service.CartService.Application.Services;
using cart_service.CartService.Infastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using productService.API.Protos;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace cart_service.CartService.API.CartControllers
{
    [Route("api/cart")]
    [ApiController]
    public class FoodCartController : ControllerBase
    {
        private readonly FoodProductsDbContext _db;
        private readonly ICreateNewCart _cart;
        private readonly CartServiceClient _product;
        private readonly IUpdateCartFood _updateCart;

        public FoodCartController(FoodProductsDbContext foodProductsDbContext, ICreateNewCart createNewCart, CartServiceClient cartServiceClient, IUpdateCartFood updateCartFood)
        {
            _db = foodProductsDbContext;
            _cart = createNewCart;
            _product = cartServiceClient;
            _updateCart = updateCartFood;
        }

        [HttpGet("test")]
        public IActionResult test()
        {

            var result = _db.Carts.ToList();
            return Ok(result);
        }

        // đã test
        [HttpPost]
        public async Task<IActionResult> TestCreateCart([FromBody] RequestCreateNewCartUser request)
        {
            var idCart = await _cart.Excute(request);
            return Ok(idCart);
        }


        // test gprc
        [HttpPost("products")]
        public async Task<IActionResult> UpdateCartFood([FromBody] ProductIDInternal request)
        {
            var grpcRequest = new ProductRequestList();

            grpcRequest.ProductId.AddRange(
                request.IdProduct.Select(id => id.ToString())
            );

            var grpcResponse = await _product.GetProductInfoAsync(grpcRequest);
            return Ok(grpcResponse);
        }

        [HttpPost("update-cart")]
        public async Task<IActionResult> TestAddProductIntoCart([FromBody]RequestUpdateCartFood request)
        {
            await _updateCart.Excute(request);
            return Ok();
        }

    }
}
