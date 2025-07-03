// File: Services/CsvOrderService.cs
using HotelManagement.WebApi.Models;
using System.Globalization;
using System.Text;

namespace HotelManagement.WebApi.Data
{
    public class CsvOrderService : IOrderService
    {
        private readonly string _csvPath;

        public CsvOrderService(string csvPath)
        {
            _csvPath = csvPath;
            EnsureCsvExists();
        }
              

        private void EnsureCsvExists()
        {
            try
            {
                if (!File.Exists(_csvPath))
                {
                    var directory = Path.GetDirectoryName(_csvPath);
                    if (!string.IsNullOrWhiteSpace(directory))
                        Directory.CreateDirectory(directory);

                    File.WriteAllText(_csvPath,
                        "OrderId,MenuItemId,CustomerName,CustomerEmail,CustomerMobile,ItemQuantity,TotalPrice,PurchaseDate,CreatedById,CreatedOn,UpdatedById,UpdatedOn,Active\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Csv Init Error] Path: {_csvPath}, Error: {ex.Message}");
                throw; // Consider rethrowing or logging via a logger if possible
            }
        }


        public List<OrderMaster> ReadOrders()
        {
            var orders = new List<OrderMaster>();

            if (!File.Exists(_csvPath))
                return orders;

            var lines = File.ReadAllLines(_csvPath).Skip(1); // Skip header

            foreach (var line in lines)
            {
                var values = line.Split(',');

                if (values.Length < 13) continue;

                orders.Add(new OrderMaster
                {
                    OrderId = int.Parse(values[0]),
                    MenuItemId = int.Parse(values[1]),
                    CustomerName = values[2],
                    CustomerEmail = values[3],
                    CustomerMobile = decimal.Parse(values[4], CultureInfo.InvariantCulture),
                    ItemQuantity = decimal.Parse(values[5], CultureInfo.InvariantCulture),
                    TotalPrice = double.Parse(values[6], CultureInfo.InvariantCulture),
                    PurchaseDate = DateTime.Parse(values[7], CultureInfo.InvariantCulture),
                    CreatedById = int.Parse(values[8]),
                    CreatedOn = DateTime.Parse(values[9], CultureInfo.InvariantCulture),
                    UpdatedById = int.Parse(values[10]),
                    UpdatedOn = string.IsNullOrEmpty(values[11]) ? (DateTime?)null : DateTime.Parse(values[11], CultureInfo.InvariantCulture),
                    Active = bool.Parse(values[12])
                });
            }

            return orders;
        }

        public void WriteOrder(OrderMaster order)
        {
            bool fileExists = File.Exists(_csvPath);

            var csvLine = $"{order.OrderId},{order.MenuItemId},{order.CustomerName},{order.CustomerEmail},{order.CustomerMobile},{order.ItemQuantity},{order.TotalPrice},{order.PurchaseDate},{order.CreatedById},{order.CreatedOn},{order.UpdatedById},{order.UpdatedOn},{order.Active}";

            using (var writer = new StreamWriter(_csvPath, append: true))
            {
                if (!fileExists)
                {
                    writer.WriteLine("OrderId,MenuItemId,CustomerName,CustomerEmail,CustomerMobile,ItemQuantity,TotalPrice,PurchaseDate,CreatedById,CreatedOn,UpdatedById,UpdatedOn,Active");
                }

                writer.WriteLine(csvLine); // ✅ Ensure each order is on a new line
            }
        }

        public void WriteAllOrders(List<OrderMaster> orders)
        {
            using (var writer = new StreamWriter(_csvPath, false, Encoding.UTF8))
            {
                writer.WriteLine("OrderId,MenuItemId,CustomerName,CustomerEmail,CustomerMobile,ItemQuantity,TotalPrice,PurchaseDate,CreatedById,CreatedOn,UpdatedById,UpdatedOn,Active");

                foreach (var order in orders)
                {
                    var csvLine = $"{order.OrderId},{order.MenuItemId},{order.CustomerName},{order.CustomerEmail},{order.CustomerMobile},{order.ItemQuantity},{order.TotalPrice},{order.PurchaseDate},{order.CreatedById},{order.CreatedOn},{order.UpdatedById},{order.UpdatedOn},{order.Active}";
                    writer.WriteLine(csvLine);
                }
            }
        }

        public void UpdateOrder(OrderMaster updatedOrder)
        {
            var orders = ReadOrders();
            var index = orders.FindIndex(o => o.OrderId == updatedOrder.OrderId);

            if (index != -1)
            {
                orders[index] = updatedOrder;
                WriteAllOrders(orders);
            }
        }

        public void DeleteOrder(int orderId)
        {
            var orders = ReadOrders();
            var updatedList = orders.Where(o => o.OrderId != orderId).ToList();
            WriteAllOrders(updatedList);
        }
    }
}
