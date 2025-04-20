namespace HotelManagement.ConsoleAppTester
{
    using Npgsql;
    internal class Program
    {
        static void Main(string[] args)
        {
            var cs1 = "Host=[2a05:d016:571:a404:e7c0:a14a:61cc:50ed];Port=5432;Database=HotelManagement;Username=postgres;Password=Gorakh@2015;Ssl Mode=Require;Trust Server Certificate=true";
            var cs2 = "Host=db.orcillprmncaasoxgqfo.supabase.co;Port=5432;Username=postgres;Password=Gorakh@2015;Database=postgres;SslMode=Require;Trust Server Certificate=true;";


            //var cs = "postgresql://postgres.orcillprmncaasoxgqfo:[Gorakh@2015]@aws-0-eu-north-1.pooler.supabase.com:6543/postgres";
            var cs = "Host=aws-0-eu-north-1.pooler.supabase.com;Port=6543;Username=postgres.orcillprmncaasoxgqfo;Password=Gorakh@2015;Database=postgres;SslMode=Require;Trust Server Certificate=true;";

            try
            {
                using var con = new NpgsqlConnection(cs);
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
