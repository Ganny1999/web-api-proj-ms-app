using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using order_place_api_ms.DomainModel.Dtos;
using order_place_api_ms.DomainModel.IServices;
using order_place_api_ms.DomainModel.Models;

namespace order_place_api_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
                _orderService= orderService;
        }

        [HttpPost("PlaceOrder/{CustomerID:int}")]
        public async Task<ActionResult<Order>> PlaceOrder(int CustomerID)
        {
           var result = await _orderService.PlaceOrderAsync(CustomerID);

            return result;
        }
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<Order>> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<ActionResult<Order>> GetOrderByID(Guid orderID)
        {

            var result = await _orderService.GetOrderByOrderIdAsync(orderID);
            if(result != null)
            {
                return Ok(result);
            }
            return NoContent();
        }
    }
}
