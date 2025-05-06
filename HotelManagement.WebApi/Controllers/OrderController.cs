using HotelManagement.WebApi.Data;
using HotelManagement.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = CsvOrderService.ReadOrders();
            return Ok(orders);
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody] OrderMaster order)
        {
            var existingOrders = CsvOrderService.ReadOrders();
            order.OrderId = existingOrders.Count > 0 ? existingOrders.Max(o => o.OrderId) + 1 : 1;
            order.CreatedOn = DateTime.UtcNow;

            CsvOrderService.WriteOrder(order);
            return CreatedAtAction(nameof(GetAll), new { id = order.OrderId }, order);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderMaster updatedOrder)
        {
            var orders = CsvOrderService.ReadOrders();
            var existingOrder = orders.FirstOrDefault(o => o.OrderId == id);

            if (existingOrder == null)
                return NotFound($"Order with ID {id} not found.");

            // Update fields (you could also just replace the whole object if needed)
            updatedOrder.OrderId = id;
            updatedOrder.CreatedOn = existingOrder.CreatedOn; // preserve created timestamp
            updatedOrder.CreatedById = existingOrder.CreatedById;
            updatedOrder.UpdatedOn = DateTime.UtcNow;

            CsvOrderService.UpdateOrder(updatedOrder);
            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var orders = CsvOrderService.ReadOrders();
            var order = orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            CsvOrderService.DeleteOrder(id);
            return NoContent();
        }

    }

}
