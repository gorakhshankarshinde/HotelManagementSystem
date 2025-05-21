using HotelManagement.WebApi.Models;
using System.Globalization;
using System.Text;

namespace HotelManagement.WebApi.Data
{
    public static class CsvMenuItemService
    {
        // Path to writable runtime CSV
        private static readonly string RuntimeCsvPath = "/tmp/MenuItems.csv";

        // Path to read-only default CSV in published output
        private static readonly string SourceCsvPath = Path.Combine(
            AppContext.BaseDirectory,
            "App_Data",
            "MenuItems.csv"
        );

        // Ensure the CSV exists in /tmp on startup
        static CsvMenuItemService()
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
                        // Create new CSV with header if default is missing
                        File.WriteAllText(RuntimeCsvPath, "MenuItemId,MenuItemName,MenuItemType,Price\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Csv Init Error] {ex.Message}");
            }
        }

        public static List<MenuItem> ReadMenuItems()
        {
            var menuItems = new List<MenuItem>();

            if (!File.Exists(RuntimeCsvPath))
                return menuItems;

            var lines = File.ReadAllLines(RuntimeCsvPath).Skip(1); // skip header

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length < 4) continue;

                menuItems.Add(new MenuItem
                {
                    MenuItemId = int.Parse(parts[0]),
                    MenuItemName = parts[1],
                    MenuItemType = parts[2],
                    Price = decimal.Parse(parts[3], CultureInfo.InvariantCulture)
                });
            }

            return menuItems;
        }

        public static void WriteMenuItem(MenuItem item)
        {
            bool fileExists = File.Exists(RuntimeCsvPath);
            using (var writer = new StreamWriter(RuntimeCsvPath, append: true))
            {
                if (!fileExists)
                {
                    writer.WriteLine("MenuItemId,MenuItemName,MenuItemType,Price");
                }

                writer.WriteLine($"{item.MenuItemId},{item.MenuItemName},{item.MenuItemType},{item.Price}");
            }
        }

        public static void UpdateMenuItem(MenuItem updatedItem)
        {
            var items = ReadMenuItems();
            var index = items.FindIndex(m => m.MenuItemId == updatedItem.MenuItemId);

            if (index == -1) return;

            items[index] = updatedItem;

            OverwriteCsv(items);
        }

        public static void DeleteMenuItem(int id)
        {
            var items = ReadMenuItems();
            items = items.Where(m => m.MenuItemId != id).ToList();

            OverwriteCsv(items);
        }

        private static void OverwriteCsv(List<MenuItem> items)
        {
            using (var writer = new StreamWriter(RuntimeCsvPath, false, Encoding.UTF8))
            {
                writer.WriteLine("MenuItemId,MenuItemName,MenuItemType,Price");
                foreach (var item in items)
                {
                    writer.WriteLine($"{item.MenuItemId},{item.MenuItemName},{item.MenuItemType},{item.Price}");
                }
            }
        }
    }
}
