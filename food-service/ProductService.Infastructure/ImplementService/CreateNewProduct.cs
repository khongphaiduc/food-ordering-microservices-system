using food_service.ProductService.Application.DTOs.Request;
using food_service.ProductService.Application.Service;
using food_service.ProductService.Domain.Aggragate;
using food_service.ProductService.Domain.Interface;
using food_service.ProductService.Domain.ValueOject;

namespace food_service.ProductService.Infastructure.ImplementService
{
    public class CreateNewProduct : ICreateNewProduct
    {
        private readonly IProductRepository _iProductRepository;

        public CreateNewProduct(IProductRepository productRepository)
        {
            _iProductRepository = productRepository;
        }

        public async Task<bool> ExcuteAsync(CreateNewProducDTO request)
        {

            var productAggregate = ProductAggregate.CreateNewProduct(request.IdCategory, new Name(request.Name), new Price(request.Price), request.Description);

            var resultAdd = await _iProductRepository.AddProductAsync(productAggregate);

            return resultAdd;
        }
    }
}
