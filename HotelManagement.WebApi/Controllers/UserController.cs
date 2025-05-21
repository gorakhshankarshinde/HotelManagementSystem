using HotelManagement.WebApi.Data;
using HotelManagement.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = CsvUserService.ReadUsers();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] UserMaster user)
        {
            var existingUsers = CsvUserService.ReadUsers();
            user.UserId = existingUsers.Count > 0 ? existingUsers.Max(u => u.UserId) + 1 : 1;
            user.CreatedOn = DateTime.UtcNow;
            user.UpdatedOn = DateTime.UtcNow;

            CsvUserService.WriteUser(user);
            return CreatedAtAction(nameof(GetAll), new { id = user.UserId }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserMaster updatedUser)
        {
            var users = CsvUserService.ReadUsers();
            var existingUser = users.FirstOrDefault(u => u.UserId == id);

            if (existingUser == null)
                return NotFound($"User with ID {id} not found.");

            updatedUser.UserId = id;
            updatedUser.CreatedOn = existingUser.CreatedOn;
            updatedUser.CreatedBy = existingUser.CreatedBy;
            updatedUser.UpdatedOn = DateTime.UtcNow;

            CsvUserService.UpdateUser(updatedUser);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var users = CsvUserService.ReadUsers();
            var user = users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            CsvUserService.DeleteUser(id);
            return NoContent();
        }
    }
}
