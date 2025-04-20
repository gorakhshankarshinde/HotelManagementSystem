namespace HotelManagement.ConsoleAppTester
{
    using Npgsql;
    using DotNetEnv;
    using System;
    using System.IO;

    internal class Program
    {
        static void Main(string[] args)
        {
            // Load .env file from the project root
            var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");
            Env.Load(envPath);

            // Get env variables
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var pass = Environment.GetEnvironmentVariable("DB_PASS");
            var dbname = Environment.GetEnvironmentVariable("DB_NAME");

            // Build connection string
            var connectionString = $"Host={host};Port={port};Username={user};Password={pass};Database={dbname};SslMode=Require;Trust Server Certificate=true;";

            try
            {
                using var con = new NpgsqlConnection(connectionString);
                con.Open();
                Console.WriteLine("Connected to Supabase PostgreSQL Successfully...!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        }
    }
}
