using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using search_service.SearchService.Application.Interface;
using System.Threading.Tasks;

namespace search_service.SearchService.API
{
    [Route("api/search")]
    [ApiController]
    public class SearchProductController : ControllerBase
    {
        private readonly ILoadFullProduct _test;

        public SearchProductController(ILoadFullProduct loadFullProduct)
        {
            _test = loadFullProduct;
        }

        [HttpGet]
        public async Task<IActionResult> Transf()
        {
            var result = await _test.LoadFullProductAsync();
            return Ok(result);
        }
    }
}
