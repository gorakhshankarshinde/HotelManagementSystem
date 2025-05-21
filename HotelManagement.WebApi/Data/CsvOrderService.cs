namespace HotelManagement.WebApi.Data
{
    using HotelManagement.WebApi.Models;
    using System.Text;

    public static class CsvOrderService
    {

        //private static readonly string csvPath = Path.Combine(Directory.GetCurrentDirectory(), "AppData", "orders.csv");
        //private static readonly string csvPath = "AppData/orders.csv";
        

        //private static readonly string csvPath = Path.Combine(
        //                                                       AppContext.BaseDirectory,
        //                                                       "App_Data",
        //                                                       "orders.csv"
        //                                                      );

        private static readonly string csvPath = Path.Combine(AppContext.BaseDirectory, "App_Data", "orders.csv");


        public static List<OrderMaster> ReadOrders()
        {
            var orders = new List<OrderMaster>();

            if (!File.Exists(csvPath))
                return orders;

            var lines = File.ReadAllLines(csvPath).Skip(1); // Skip header

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
            bool fileExists = File.Exists(csvPath);

            var csvLine = $"{order.OrderId},{order.MenuItemId},{order.CustomerName},{order.CustomerEmail},{order.CustomerMobile},{order.ItemQuantity},{order.TotalPrice},{order.PurchaseDate},{order.CreatedById},{order.CreatedOn},{order.UpdatedById},{order.UpdatedOn},{order.Active}";

            using (var writer = new StreamWriter(csvPath, append: true))
            {
                if (!fileExists)
                {
                    // Write header
                    writer.WriteLine("OrderId,MenuItemId,CustomerName,CustomerEmail,CustomerMobile,ItemQuantity,TotalPrice,PurchaseDate,CreatedById,CreatedOn,UpdatedById,UpdatedOn,Active");
                }

                writer.WriteLine(csvLine);
            }
        }

        public static void WriteAllOrders(List<OrderMaster> orders)
        {
            using (var writer = new StreamWriter(csvPath, false))
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
