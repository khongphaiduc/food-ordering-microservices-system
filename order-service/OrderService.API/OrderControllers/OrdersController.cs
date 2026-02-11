using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using order_service.OrderService.Appilcation.DTOs;
using order_service.OrderService.Appilcation.Services;
using order_service.OrderService.Domain.Enums;
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
        private readonly IGetListOrderOfUser _getListOrder;

        public OrdersController(FoodOrderContext foodOrderContext, ICreateNewOrder createNewOrder, IGetListOrderOfUser getListOrderOfUser)
        {
            _db = foodOrderContext;
            _order = createNewOrder;
            _getListOrder = getListOrderOfUser;
        }


        // tạo order 
        [HttpPost]
        public async Task<IActionResult> CreateNewOrder([FromBody] RequestPaymentCart request)
        {
            PaymentMethod methodPayment = (PaymentMethod)request.PaymentMethod;
            var QRCodeString = await _order.Excute(request.IdCart, methodPayment);
            return Ok(QRCodeString);
        }


        // xem danh sách order 
        [HttpPost("histories")]
        public async Task<IActionResult> GetListOrders([FromBody] RequestGetListOrderWithPagination request)
        {
            var orders = await _getListOrder.GetListOrderForUser(request);
            return Ok(orders);
        }

    }
}