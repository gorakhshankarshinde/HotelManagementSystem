using HotelManagement.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace HotelManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderMasterController : ControllerBase
    {
        private readonly DatabaseHelper _dbHelper;

        private readonly IConfiguration _configuration;

        // Initialize the helper with the connection string
        public OrderMasterController(IConfiguration configuration)
        {
            _configuration = configuration;

            // Read the connection string from appsettings.json
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            _dbHelper = new DatabaseHelper(connectionString);
        }

        // GET: api/OrderMaster
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            string query = "SELECT * FROM public.\"Tbl_OrderMaster\"";  // Query to get all orders

            var orders = await _dbHelper.ExecuteQueryAsync<OrderMaster>(query, reader =>
            {
                return new OrderMaster
                {
                    OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                    MenuItemId = reader.GetInt32(reader.GetOrdinal("MenuItemId")),
                    CustomerMobile = reader.GetInt64(reader.GetOrdinal("CustomerMobile")),
                    ItemQuantity = reader.GetInt32(reader.GetOrdinal("ItemQuantity")),
                    TotalPrice = reader.GetDouble(reader.GetOrdinal("TotalPrice")),
                    PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                    CreatedById = reader.GetInt32(reader.GetOrdinal("CreatedById")),
                    CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                    UpdatedById = reader.IsDBNull(reader.GetOrdinal("UpdatedById")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("UpdatedById")),
                    UpdatedOn = reader.IsDBNull(reader.GetOrdinal("UpdatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedOn")),
                    Active = reader.GetBoolean(reader.GetOrdinal("Active"))
                };
            });

            return Ok(orders);
        }

        // Example for INSERT operation
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderMaster order)
        {
            string query = "INSERT INTO public.\"Tbl_OrderMaster\" (\"MenuItemId\", \"CustomerMobile\", \"ItemQuantity\", \"TotalPrice\", \"PurchaseDate\", \"CreatedById\", \"CreatedOn\", \"Active\") " +
                           "VALUES (@MenuItemId, @CustomerMobile, @ItemQuantity, @TotalPrice, @PurchaseDate, @CreatedById, @CreatedOn, @Active)";

            var command = new NpgsqlCommand(query);
            command.Parameters.AddWithValue("@MenuItemId", order.MenuItemId);
            command.Parameters.AddWithValue("@CustomerMobile", order.CustomerMobile);
            command.Parameters.AddWithValue("@ItemQuantity", order.ItemQuantity);
            command.Parameters.AddWithValue("@TotalPrice", order.TotalPrice);
            command.Parameters.AddWithValue("@PurchaseDate", order.PurchaseDate);
            command.Parameters.AddWithValue("@CreatedById", order.CreatedById);
            command.Parameters.AddWithValue("@CreatedOn", DateTime.Now); // Automatically set current timestamp
            command.Parameters.AddWithValue("@Active", order.Active);

            var affectedRows = await _dbHelper.ExecuteNonQueryAsync(command.CommandText);

            if (affectedRows > 0)
                return Ok("Order created successfully.");
            else
                return BadRequest("Failed to create order.");
        }
    }

    public class OrderMaster
    {
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public long CustomerMobile { get; set; }
        public int ItemQuantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Active { get; set; }
    }
}
