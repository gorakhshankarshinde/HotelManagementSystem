using HotelManagement.WebApi.Data;
using HotelManagement.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace HotelManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderMasterController : ControllerBase
    {
        private readonly DatabaseHelper _dbHelper;

        public OrderMasterController(IConfiguration configuration)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var pass = Environment.GetEnvironmentVariable("DB_PASS");
            var dbname = Environment.GetEnvironmentVariable("DB_NAME");

            var connectionString = $"Host={host};Port={port};Username={user};Password={pass};Database={dbname};SslMode=Require;Trust Server Certificate=true;";

            _dbHelper = new DatabaseHelper(connectionString);
        }

        // GET: api/OrderMaster
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            string query = "SELECT * FROM public.\"VW_OrderDetails\"";  // <-- Now querying the VIEW



            var orders = await _dbHelper.ExecuteQueryAsync<OrderDetails>(query, reader =>
            {
                return new OrderDetails
                {
                    OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                    CustomerEmail = reader.IsDBNull(reader.GetOrdinal("CustomerEmail")) ? null : reader.GetString(reader.GetOrdinal("CustomerEmail")),
                    CustomerMobile = reader.GetInt64(reader.GetOrdinal("CustomerMobile")),
                    ItemQuantity = reader.GetInt32(reader.GetOrdinal("ItemQuantity")),
                    TotalPrice = reader.GetDouble(reader.GetOrdinal("TotalPrice")),
                    PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                    CreatedById = reader.GetInt32(reader.GetOrdinal("CreatedById")),
                    CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                    UpdatedById = reader.IsDBNull(reader.GetOrdinal("UpdatedById")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("UpdatedById")),
                    UpdatedOn = reader.IsDBNull(reader.GetOrdinal("UpdatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedOn")),
                    OrderActive = reader.GetBoolean(reader.GetOrdinal("OrderActive")),

                    MenuItemId = reader.GetInt32(reader.GetOrdinal("MenuItemId")),
                    MenuItemName = reader.GetString(reader.GetOrdinal("MenuItemName")),
                    MenuItemPrice = reader.GetDouble(reader.GetOrdinal("MenuItemPrice")),
                    MenuItemActive = reader.GetBoolean(reader.GetOrdinal("MenuItemActive")),

                    MenuItemTypeId = reader.GetInt32(reader.GetOrdinal("MenuItemTypeId")),
                    MenuItemType = reader.GetString(reader.GetOrdinal("MenuItemType")),
                    MenuItemTypeActive = reader.GetBoolean(reader.GetOrdinal("MenuItemTypeActive"))
                };
            });

            return Ok(orders);
        }

    }

    
}
