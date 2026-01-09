using food_service.productservice.domain.valueobject.domain;
using food_service.productservice.infastructure.Models;

namespace food_service.productservice.domain.entities.domain
{
    public class ProductsEntity
    {
        public Guid Id { get; }
        public Name NameValue { get; private set; }
        public Price PriceValue { get; private set; }


    }
}
