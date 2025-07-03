using HotelManagement.WebApi.Models;
using System.Globalization;
using System.Text;

namespace HotelManagement.WebApi.Data
{
    public class CsvMenuItemService : IMenuItemService
    {
        private readonly string _csvPath;

        // ✅ Constructor that takes a custom path
        public CsvMenuItemService(string runtimeCsvPath)
        {
            _csvPath = runtimeCsvPath;
            EnsureCsvExists();
        }

        private void EnsureCsvExists()
        {
            try
            {
                if (!File.Exists(_csvPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_csvPath)!);

                    var sourcePath = Path.Combine(AppContext.BaseDirectory, "App_Data", "MenuItems.csv");

                    if (File.Exists(sourcePath))
                    {
                        File.Copy(sourcePath, _csvPath);
                    }
                    else
                    {
                        File.WriteAllText(_csvPath, "MenuItemId,MenuItemName,MenuItemType,Price\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Csv Init Error] {ex.Message}");
            }
        }

        public List<MenuItem> ReadMenuItems()
        {
            if (!File.Exists(_csvPath)) return new List<MenuItem>();

            return File.ReadAllLines(_csvPath)
                       .Skip(1)
                       .Select(line => line.Split(','))
                       .Where(parts => parts.Length >= 4)
                       .Select(parts => new MenuItem
                       {
                           MenuItemId = int.Parse(parts[0]),
                           MenuItemName = parts[1],
                           MenuItemType = parts[2],
                           Price = decimal.Parse(parts[3], CultureInfo.InvariantCulture)
                       })
                       .ToList();
        }

        public void WriteMenuItem(MenuItem item)
        {
            bool fileExists = File.Exists(_csvPath);

            using (var writer = new StreamWriter(_csvPath, append: true))
            {
                if (!fileExists)
                {
                    writer.WriteLine("MenuItemId,MenuItemName,MenuItemType,Price");
                }

                // ✅ Ensure the record is written on a new line
                writer.WriteLine($"{item.MenuItemId},{item.MenuItemName},{item.MenuItemType},{item.Price}");
            }
        }


        public void UpdateMenuItem(MenuItem updatedItem)
        {
            var items = ReadMenuItems();
            var index = items.FindIndex(m => m.MenuItemId == updatedItem.MenuItemId);
            if (index == -1) return;

            items[index] = updatedItem;
            OverwriteCsv(items);
        }

        public void DeleteMenuItem(int id)
        {
            var items = ReadMenuItems().Where(m => m.MenuItemId != id).ToList();
            OverwriteCsv(items);
        }

        private void OverwriteCsv(List<MenuItem> items)
        {
            using var writer = new StreamWriter(_csvPath, false, Encoding.UTF8);
            writer.WriteLine("MenuItemId,MenuItemName,MenuItemType,Price");
            foreach (var item in items)
            {
                writer.WriteLine($"{item.MenuItemId},{item.MenuItemName},{item.MenuItemType},{item.Price}");
            }
        }
    }
}
