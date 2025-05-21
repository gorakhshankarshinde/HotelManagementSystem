using HotelManagement.WebApi.Models;
using System.Globalization;
using System.Text;

namespace HotelManagement.WebApi.Data
{
    public static class CsvUserService
    {
        private static readonly string RuntimeCsvPath = "/tmp/Users.csv";
        private static readonly string SourceCsvPath = Path.Combine(AppContext.BaseDirectory, "App_Data", "Users.csv");

        static CsvUserService()
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
                        File.WriteAllText(RuntimeCsvPath, "UserId,UserFirstName,UserLastName,Email,Password,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,Active\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Csv Init Error] {ex.Message}");
            }
        }

        public static List<UserMaster> ReadUsers()
        {
            var users = new List<UserMaster>();

            if (!File.Exists(RuntimeCsvPath))
                return users;

            var lines = File.ReadAllLines(RuntimeCsvPath).Skip(1); // skip header

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length < 10) continue;

                users.Add(new UserMaster
                {
                    UserId = int.Parse(parts[0]),
                    UserFirstName = parts[1],
                    UserLastName = parts[2],
                    Email = parts[3],
                    Password = parts[4],
                    CreatedBy = int.Parse(parts[5]),
                    CreatedOn = DateTime.Parse(parts[6]),
                    UpdatedBy = int.Parse(parts[7]),
                    UpdatedOn = DateTime.Parse(parts[8]),
                    Active = bool.Parse(parts[9])
                });
            }

            return users;
        }

        public static void WriteUser(UserMaster user)
        {
            bool fileExists = File.Exists(RuntimeCsvPath);

            var line = $"{user.UserId},{user.UserFirstName},{user.UserLastName},{user.Email},{user.Password},{user.CreatedBy},{user.CreatedOn},{user.UpdatedBy},{user.UpdatedOn},{user.Active}";

            using (var writer = new StreamWriter(RuntimeCsvPath, append: true))
            {
                if (!fileExists)
                {
                    writer.WriteLine("UserId,UserFirstName,UserLastName,Email,Password,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,Active");
                }

                writer.WriteLine(line);
            }
        }

        public static void WriteAllUsers(List<UserMaster> users)
        {
            using (var writer = new StreamWriter(RuntimeCsvPath, false, Encoding.UTF8))
            {
                writer.WriteLine("UserId,UserFirstName,UserLastName,Email,Password,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,Active");

                foreach (var user in users)
                {
                    var line = $"{user.UserId},{user.UserFirstName},{user.UserLastName},{user.Email},{user.Password},{user.CreatedBy},{user.CreatedOn},{user.UpdatedBy},{user.UpdatedOn},{user.Active}";
                    writer.WriteLine(line);
                }
            }
        }

        public static void UpdateUser(UserMaster updatedUser)
        {
            var users = ReadUsers();
            var index = users.FindIndex(u => u.UserId == updatedUser.UserId);

            if (index != -1)
            {
                users[index] = updatedUser;
                WriteAllUsers(users);
            }
        }

        public static void DeleteUser(int userId)
        {
            var users = ReadUsers();
            var updatedList = users.Where(u => u.UserId != userId).ToList();
            WriteAllUsers(updatedList);
        }
    }
}
