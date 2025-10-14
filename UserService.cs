using System.Collections.Generic;
using System.Linq;

namespace MauiApp3
{
    public static class UserService
    {
        private static List<User> users = new List<User>();
        private static readonly string usersFilePath = Path.Combine(FileSystem.AppDataDirectory, "users.txt");

        // Load users from file when the service is first used
        static UserService()
        {
            LoadUsersFromFile();
        }

        private static void LoadUsersFromFile()
        {
            try
            {
                if (File.Exists(usersFilePath))
                {
                    var lines = File.ReadAllLines(usersFilePath);
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var parts = line.Split('|');
                        if (parts.Length >= 3)
                        {
                            users.Add(new User
                            {
                                Username = parts[0],
                                Email = parts[1],
                                Password = parts[2],
                                Role = parts.Length > 3 ? parts[3] : "Student",
                                CreatedDate = parts.Length > 4 && DateTime.TryParse(parts[4], out var date)
                                    ? date
                                    : DateTime.Now
                            });
                        }
                    }
                }
                else
                {
                    // Create default users if file doesn't exist
                    users.Add(new User { Username = "admin", Email = "admin@example.com", Password = "admin123", Role = "Staff" });
                    users.Add(new User { Username = "staff", Email = "staff@example.com", Password = "staff123", Role = "Staff" });
                    users.Add(new User { Username = "demo", Email = "demo@example.com", Password = "demo123", Role = "Student" });
                    SaveUsersToFile();
                }
            }
            catch (Exception ex)
            {
                // If loading fails, use default users
                users.Add(new User { Username = "admin", Email = "admin@example.com", Password = "admin123", Role = "Staff" });
                users.Add(new User { Username = "staff", Email = "staff@example.com", Password = "staff123", Role = "Staff" });
                users.Add(new User { Username = "demo", Email = "demo@example.com", Password = "demo123", Role = "Student" });
            }
        }

        private static void SaveUsersToFile()
        {
            try
            {
                var lines = users.Select(u => $"{u.Username}|{u.Email}|{u.Password}|{u.Role}|{u.CreatedDate:O}");
                File.WriteAllLines(usersFilePath, lines);
            }
            catch (Exception ex)
            {
                // Log error if needed
            }
        }

        public static List<User> GetUsers()
        {
            return users;
        }

        public static bool UserExists(string username)
        {
            return users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public static void AddUser(User user)
        {
            users.Add(user);
            SaveUsersToFile();
        }

        public static User GetUser(string username)
        {
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public static bool ValidateUser(string username, string password)
        {
            return users.Any(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }

        // New method to check if user is staff
        public static bool IsStaff(string username)
        {
            var user = GetUser(username);
            return user?.Role == "Staff";
        }
    }
}