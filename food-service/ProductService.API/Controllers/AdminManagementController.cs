using food_service.ProductService.Application.DTOs.Request;
using food_service.ProductService.Application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace food_service.ProductService.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class AdminManagementController : ControllerBase
    {
        private readonly ICreateNewProduct _iAddNewProduct;

        public AdminManagementController(ICreateNewProduct createNewProduct)
        {
            _iAddNewProduct = createNewProduct;
        }


        // đã test
        [HttpPost]
        public async Task<ActionResult> CreateNewProduct([FromBody] CreateNewProducDTO request)
        {
            var result = await _iAddNewProduct.ExcuteAsync(request);
            return Ok(result);
        }

    }
}
