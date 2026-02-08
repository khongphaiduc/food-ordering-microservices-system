using cart_service.CartService.Application.DTOInternal;
using Google.Protobuf;
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


        public async Task<List<ProductInforgetImage>> GetProductImage(List<GetUrlImageProduct> request)
        {
            var grpcRequest = new GetImageProductsRequest();

            grpcRequest.IdProduct.AddRange(
                request.Select(x => x.IdProduct.ToString())
            );

            var result = await _productInfoGrpcClient.GetImageProductsAsync(
                grpcRequest,
                deadline: DateTime.UtcNow.AddSeconds(5)
            );

            var productResult = result.ProductInfors.Select(s => new ProductInforgetImage
            {
                IdProduct = Guid.Parse(s.IdProduct),
                UrlImage = s.UrlImage
            }).ToList();

            return productResult;
        }


    }
}
