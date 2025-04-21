using HotelManagement.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace HotelManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly DatabaseHelper _dbHelper;

        public MenuItemController(IConfiguration configuration)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var pass = Environment.GetEnvironmentVariable("DB_PASS");
            var dbname = Environment.GetEnvironmentVariable("DB_NAME");

            var connectionString = $"Host={host};Port={port};Username={user};Password={pass};Database={dbname};SslMode=Require;Trust Server Certificate=true;";
            _dbHelper = new DatabaseHelper(connectionString);
        }

        // GET: api/MenuItem
        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            string query = "SELECT * FROM public.\"VW_MenuItemsWithType\"";

            var items = await _dbHelper.ExecuteQueryAsync<MenuItemWithType>(query, reader =>
            {
                return new MenuItemWithType
                {
                    MenuItemId = reader.GetInt32(reader.GetOrdinal("MenuItemId")),
                    MenuItemName = reader.GetString(reader.GetOrdinal("MenuItemName")),
                    MenuItemTypeId = reader.GetInt32(reader.GetOrdinal("MenuItemTypeId")),
                    MenuItemType = reader.GetString(reader.GetOrdinal("MenuItemType")),
                    Price = reader.GetDouble(reader.GetOrdinal("Price")),
                    MenuItemActive = reader.GetBoolean(reader.GetOrdinal("MenuItemActive")),
                    MenuItemTypeActive = reader.GetBoolean(reader.GetOrdinal("MenuItemTypeActive"))
                };
            });

            return Ok(items);
        }
    }

    public class MenuItemWithType
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public int MenuItemTypeId { get; set; }
        public string MenuItemType { get; set; }
        public double Price { get; set; }
        public bool MenuItemActive { get; set; }
        public bool MenuItemTypeActive { get; set; }
    }
}
