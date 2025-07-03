using HotelManagement.WebApi.Data;
using HotelManagement.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HotelManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _menuItemService.ReadMenuItems();
            return Ok(items);
        }

        [HttpPost]
        public IActionResult AddMenuItem([FromBody] MenuItem item)
        {
            var items = _menuItemService.ReadMenuItems();
            item.MenuItemId = items.Count > 0 ? items.Max(i => i.MenuItemId) + 1 : 1;

            _menuItemService.WriteMenuItem(item);
            return CreatedAtAction(nameof(GetAll), new { id = item.MenuItemId }, item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMenuItem(int id, [FromBody] MenuItem updatedItem)
        {
            var items = _menuItemService.ReadMenuItems();
            var existing = items.FirstOrDefault(i => i.MenuItemId == id);

            if (existing == null)
                return NotFound($"Menu item with ID {id} not found.");

            updatedItem.MenuItemId = id;
            _menuItemService.UpdateMenuItem(updatedItem);
            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMenuItem(int id)
        {
            var items = _menuItemService.ReadMenuItems();
            if (!items.Any(i => i.MenuItemId == id))
                return NotFound($"Menu item with ID {id} not found.");

            _menuItemService.DeleteMenuItem(id);
            return NoContent();
        }
    }
}
