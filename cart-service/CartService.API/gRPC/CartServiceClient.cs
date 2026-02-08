using productService.API.Protos;

namespace cart_service.CartService.API.gRPC
{
    public class CartServiceClient
    {
        private ProductInfoGrpc.ProductInfoGrpcClient _productInfoGrpcClient;

        public CartServiceClient(ProductInfoGrpc.ProductInfoGrpcClient productInfoGrpcClient)
        {
            _productInfoGrpcClient = productInfoGrpcClient;
        }

        public async Task<ProductDetailList> GetProductInfoAsync(ProductRequestList request)
        {
            return await _productInfoGrpcClient.GetInformationProductsAsync(request, deadline: DateTime.UtcNow.AddSeconds(5));
        }
    }
}
