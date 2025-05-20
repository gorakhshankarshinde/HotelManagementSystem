using HotelManagement.WebApi.Models;
using System.Globalization;
using System.Text;

namespace HotelManagement.WebApi.Data
{
    public static class CsvMenuItemService
    {

        private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData", "MenuItems.csv");
        //private static readonly string FilePath = "Data/MenuItems.csv";

        public static List<MenuItem> ReadMenuItems()
        {
            var menuItems = new List<MenuItem>();

            if (!File.Exists(FilePath))
                return menuItems;

            var lines = File.ReadAllLines(FilePath).Skip(1); // skip header

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
            bool fileExists = File.Exists(FilePath);
            using (var writer = new StreamWriter(FilePath, append: true))
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
            using (var writer = new StreamWriter(FilePath, false, Encoding.UTF8))
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
