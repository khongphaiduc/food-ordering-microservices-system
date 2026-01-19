using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using search_service.SearchService.Application.DTO;
using search_service.SearchService.Application.Interface;
using System.Threading.Tasks;

namespace search_service.SearchService.API
{
    [Route("api/search")]
    [ApiController]
    public class SearchProductController : ControllerBase
    {
        private readonly ILoadFullProduct _test;
        private readonly IElasticsearch _elasticsearch;

        public SearchProductController(ILoadFullProduct loadFullProduct, IElasticsearch elasticsearch)
        {
            _test = loadFullProduct;
            _elasticsearch = elasticsearch;
        }

        [HttpGet]
        public async Task<IActionResult> Transf()
        {
            var result = await _test.LoadFullProductAsync();
            return Ok(result);
        }

        [HttpGet("products")]
        public async Task<IActionResult> TestSearch([FromQuery]SearchProduct request)
        {
            var result = await _elasticsearch.SearchByKey(request.Key, request.Index);

            return Ok(result);
        }
    }
}
