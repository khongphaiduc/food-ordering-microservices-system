using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using order_service.OrderService.Appilcation.Services;
using order_service.OrderService.Infastructure.Models;
using System.Threading.Tasks;

namespace order_service.OrderService.API.OrderControllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FoodOrderContext _db;
        private readonly ICreateNewOrder _order;

        public OrdersController(FoodOrderContext foodOrderContext, ICreateNewOrder createNewOrder)
        {
            _db = foodOrderContext;
            _order = createNewOrder;
        }


        [HttpGet]
        public IActionResult test()
        {
            var s = _db.Orders.ToList();
            return Ok(s);
        }

        [HttpPost("{Id}")]
        public async Task<IActionResult> CreateNewOrder([FromRoute] Guid Id)
        {
            var result = await _order.Excute(Id);
            return Ok(result);
        }
    }
}