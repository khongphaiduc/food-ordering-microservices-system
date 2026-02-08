using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using order_service.OrderService.Infastructure.Models;

namespace order_service.OrderService.API.OrderControllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FoodOrderContext _db;

        public OrdersController(FoodOrderContext foodOrderContext)
        {
            _db = foodOrderContext;
        }


        [HttpGet]
        public IActionResult test()
        {
            var s = _db.Orders.ToList();
            return Ok(s);
        }
    }
}
