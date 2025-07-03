using HotelManagement.WebApi.Data;
using HotelManagement.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _orderService.ReadOrders();
            return Ok(orders);
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody] OrderMaster order)
        {
            var existingOrders = _orderService.ReadOrders();
            order.OrderId = existingOrders.Count > 0 ? existingOrders.Max(o => o.OrderId) + 1 : 1;
            order.CreatedOn = DateTime.UtcNow;

            _orderService.WriteOrder(order);
            return CreatedAtAction(nameof(GetAll), new { id = order.OrderId }, order);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderMaster updatedOrder)
        {
            var orders = _orderService.ReadOrders();
            var existingOrder = orders.FirstOrDefault(o => o.OrderId == id);

            if (existingOrder == null)
                return NotFound($"Order with ID {id} not found.");

            updatedOrder.OrderId = id;
            updatedOrder.CreatedOn = existingOrder.CreatedOn;
            updatedOrder.CreatedById = existingOrder.CreatedById;
            updatedOrder.UpdatedOn = DateTime.UtcNow;

            _orderService.UpdateOrder(updatedOrder);
            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var orders = _orderService.ReadOrders();
            var order = orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            _orderService.DeleteOrder(id);
            return NoContent();
        }
    }
}
