using HotelManagement.WebApi.Data;
using HotelManagement.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var items = CsvMenuItemService.ReadMenuItems();
            return Ok(items);
        }

        [HttpPost]
        public IActionResult AddMenuItem([FromBody] MenuItem item)
        {
            var items = CsvMenuItemService.ReadMenuItems();
            item.MenuItemId = items.Count > 0 ? items.Max(i => i.MenuItemId) + 1 : 1;

            CsvMenuItemService.WriteMenuItem(item);
            return CreatedAtAction(nameof(GetAll), new { id = item.MenuItemId }, item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMenuItem(int id, [FromBody] MenuItem updatedItem)
        {
            var items = CsvMenuItemService.ReadMenuItems();
            var existing = items.FirstOrDefault(i => i.MenuItemId == id);

            if (existing == null)
                return NotFound($"Menu item with ID {id} not found.");

            updatedItem.MenuItemId = id;
            CsvMenuItemService.UpdateMenuItem(updatedItem);
            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMenuItem(int id)
        {
            var items = CsvMenuItemService.ReadMenuItems();
            if (!items.Any(i => i.MenuItemId == id))
                return NotFound($"Menu item with ID {id} not found.");

            CsvMenuItemService.DeleteMenuItem(id);
            return NoContent();
        }
    }
}

