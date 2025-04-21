namespace HotelManagement.WebApi.Data
{
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string query, Func<IDataReader, T> map)
        {
            var results = new List<T>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var command = new NpgsqlCommand(query, conn))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(map(reader));
                    }
                }
            }

            return results;
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var command = new NpgsqlCommand(query, conn))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Inside DatabaseHelper.cs
        public async Task<int> ExecuteNonQueryAsync(string query, NpgsqlParameter[] parameters)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddRange(parameters);
            return await cmd.ExecuteNonQueryAsync();
        }
    }

}
