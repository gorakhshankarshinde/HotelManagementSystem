using HotelManagement.WebApi.Data;
using HotelManagement.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace HotelManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseHelper _dbHelper;

        public UserController(IConfiguration configuration)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var pass = Environment.GetEnvironmentVariable("DB_PASS");
            var dbname = Environment.GetEnvironmentVariable("DB_NAME");

            var connectionString = $"Host={host};Port={port};Username={user};Password={pass};Database={dbname};SslMode=Require;Trust Server Certificate=true;";
            _dbHelper = new DatabaseHelper(connectionString);
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            string query = "SELECT * FROM public.\"VW_UserWithType\"";  // Querying the view

            var users = await _dbHelper.ExecuteQueryAsync<UserWithType>(query, reader =>
            {
                return new UserWithType
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    UserFullName = reader.GetString(reader.GetOrdinal("UserFullName")),
                    UserContactNumber = reader.GetDecimal(reader.GetOrdinal("UserContactNumber")),
                    UserEmailAddress = reader.GetString(reader.GetOrdinal("UserEmailAddress")),
                    UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                    UserType = reader.GetString(reader.GetOrdinal("UserType")),
                    UserCreatedBy = reader.GetInt32(reader.GetOrdinal("UserCreatedBy")),
                    UserCreatedOn = reader.GetDateTime(reader.GetOrdinal("UserCreatedOn")),
                    UserUpdatedBy = reader.IsDBNull(reader.GetOrdinal("UserUpdatedBy")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("UserUpdatedBy")),
                    UserUpdatedOn = reader.IsDBNull(reader.GetOrdinal("UserUpdatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UserUpdatedOn")),
                    UserActive = reader.GetBoolean(reader.GetOrdinal("UserActive")),
                    UserTypeActive = reader.GetBoolean(reader.GetOrdinal("UserTypeActive"))
                };
            });

            return Ok(users);
        }
    }


}
