namespace HotelManagement.WebApi.Data
{
    using HotelManagement.WebApi.Models;
    using System.Globalization;
    using System.Text;

    public static class CsvOrderService
    {
        // Writable path for Render's /tmp
        private static readonly string RuntimeCsvPath = "/tmp/orders.csv";

        // Read-only source in published output
        private static readonly string SourceCsvPath = Path.Combine(
            AppContext.BaseDirectory,
            "App_Data",
            "orders.csv"
        );

        // Copy CSV to /tmp at startup
        static CsvOrderService()
        {
            try
            {
                if (!File.Exists(RuntimeCsvPath))
                {
                    Directory.CreateDirectory("/tmp");

                    if (File.Exists(SourceCsvPath))
                    {
                        File.Copy(SourceCsvPath, RuntimeCsvPath);
                    }
                    else
                    {
                        // Create new file with header if missing
                        File.WriteAllText(RuntimeCsvPath,
                            "OrderId,MenuItemId,CustomerName,CustomerEmail,CustomerMobile,ItemQuantity,TotalPrice,PurchaseDate,CreatedById,CreatedOn,UpdatedById,UpdatedOn,Active\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Csv Init Error] {ex.Message}");
            }
        }

        public static List<OrderMaster> ReadOrders()
        {
            var orders = new List<OrderMaster>();

            if (!File.Exists(RuntimeCsvPath))
                return orders;

            var lines = File.ReadAllLines(RuntimeCsvPath).Skip(1); // Skip header

            foreach (var line in lines)
            {
                var values = line.Split(',');

                orders.Add(new OrderMaster
                {
                    OrderId = int.Parse(values[0]),
                    MenuItemId = int.Parse(values[1]),
                    CustomerName = values[2],
                    CustomerEmail = values[3],
                    CustomerMobile = decimal.Parse(values[4]),
                    ItemQuantity = decimal.Parse(values[5]),
                    TotalPrice = double.Parse(values[6]),
                    PurchaseDate = DateTime.Parse(values[7]),
                    CreatedById = int.Parse(values[8]),
                    CreatedOn = DateTime.Parse(values[9]),
                    UpdatedById = int.Parse(values[10]),
                    UpdatedOn = string.IsNullOrEmpty(values[11]) ? (DateTime?)null : DateTime.Parse(values[11]),
                    Active = bool.Parse(values[12])
                });
            }

            return orders;
        }

        public static void WriteOrder(OrderMaster order)
        {
            bool fileExists = File.Exists(RuntimeCsvPath);

            var csvLine = $"{order.OrderId},{order.MenuItemId},{order.CustomerName},{order.CustomerEmail},{order.CustomerMobile},{order.ItemQuantity},{order.TotalPrice},{order.PurchaseDate},{order.CreatedById},{order.CreatedOn},{order.UpdatedById},{order.UpdatedOn},{order.Active}";

            using (var writer = new StreamWriter(RuntimeCsvPath, append: true))
            {
                if (!fileExists)
                {
                    writer.WriteLine("OrderId,MenuItemId,CustomerName,CustomerEmail,CustomerMobile,ItemQuantity,TotalPrice,PurchaseDate,CreatedById,CreatedOn,UpdatedById,UpdatedOn,Active");
                }

                writer.WriteLine(csvLine);
            }
        }

        public static void WriteAllOrders(List<OrderMaster> orders)
        {
            using (var writer = new StreamWriter(RuntimeCsvPath, false))
            {
                writer.WriteLine("OrderId,MenuItemId,CustomerName,CustomerEmail,CustomerMobile,ItemQuantity,TotalPrice,PurchaseDate,CreatedById,CreatedOn,UpdatedById,UpdatedOn,Active");

                foreach (var order in orders)
                {
                    var csvLine = $"{order.OrderId},{order.MenuItemId},{order.CustomerName},{order.CustomerEmail},{order.CustomerMobile},{order.ItemQuantity},{order.TotalPrice},{order.PurchaseDate},{order.CreatedById},{order.CreatedOn},{order.UpdatedById},{order.UpdatedOn},{order.Active}";
                    writer.WriteLine(csvLine);
                }
            }
        }

        public static void UpdateOrder(OrderMaster updatedOrder)
        {
            var orders = ReadOrders();
            var index = orders.FindIndex(o => o.OrderId == updatedOrder.OrderId);

            if (index != -1)
            {
                orders[index] = updatedOrder;
                WriteAllOrders(orders);
            }
        }

        public static void DeleteOrder(int orderId)
        {
            var orders = ReadOrders();
            var updatedList = orders.Where(o => o.OrderId != orderId).ToList();
            WriteAllOrders(updatedList);
        }
    }
}
