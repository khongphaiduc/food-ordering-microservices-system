using food_service.productservice.infastructure.ProductDbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace food_service.productservice.api.productserviceControllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductViewController : ControllerBase
    {
        private readonly FoodProductsDbContext _db;

        public ProductViewController(FoodProductsDbContext dbContext)
        {
            _db = dbContext;                    
        }


        [HttpGet]
        public IActionResult GetProducts()
        {
            

            return Ok(_db.Products.ToList());
        }

    }
}
