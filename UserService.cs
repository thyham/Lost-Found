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

        //Uncomment the method below when you run this for the first time
        //public static void DeleteUsersFile()
        //{
        //    try
        //    {
        //        if (File.Exists(usersFilePath))
        //        {
        //            File.Delete(usersFilePath);
        //        }
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine($"[UserService] File not found: {usersFilePath}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"[UserService] Could not delete file: {ex.Message}");
        //    }
        //}


        private static void LoadUsersFromFile()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[UserService] Loading users from file: {usersFilePath}");

                if (File.Exists(usersFilePath))
                {
                    var lines = File.ReadAllLines(usersFilePath);
                    System.Diagnostics.Debug.WriteLine($"[UserService] Found {lines.Length} lines in users file");

                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var parts = line.Split('|');
                        if (parts.Length >= 4)
                        {
                            var user = new User
                            {
                                Id = int.TryParse(parts[0], out var id) ? id : users.Count + 1,
                                Username = parts[1],
                                Email = parts[2],
                                Password = parts[3],
                                Role = parts.Length > 4 ? parts[4] : "Student",
                                CreatedDate = parts.Length > 5 && DateTime.TryParse(parts[5], out var date)
                                    ? date
                                    : DateTime.Now
                            };
                            users.Add(user);
                            System.Diagnostics.Debug.WriteLine($"[UserService] Loaded user: {user.Username}, Role: {user.Role}, Password: {user.Password}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"[UserService] Skipping invalid line: {line}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[UserService] No existing users file found. Creating default users...");

                    // Create default users if file doesn't exist
                    users.Add(new User { Id = 1, Username = "admin", Email = "admin@example.com", Password = "admin123", Role = "Staff" });
                    users.Add(new User { Id = 2, Username = "staff", Email = "staff@example.com", Password = "staff123", Role = "Staff" });
                    users.Add(new User { Id = 3, Username = "demo", Email = "demo@example.com", Password = "demo123", Role = "Student" });
                    SaveUsersToFile();

                    System.Diagnostics.Debug.WriteLine("[UserService] Default users created and saved.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UserService] Error loading users: {ex.Message}");
                System.Diagnostics.Debug.WriteLine("[UserService] Using default fallback users.");

                // If loading fails, use default users
                users.Add(new User { Id = 1, Username = "admin", Email = "admin@example.com", Password = "admin123", Role = "Staff" });
                users.Add(new User { Id = 2, Username = "staff", Email = "staff@example.com", Password = "staff123", Role = "Staff" });
                users.Add(new User { Id = 3, Username = "demo", Email = "demo@example.com", Password = "demo123", Role = "Student" });
            }
        }

        private static void SaveUsersToFile()
        {
            try
            {
                var lines = users.Select(u => $"{u.Id}|{u.Username}|{u.Email}|{u.Password}|{u.Role}|{u.CreatedDate:O}");
                File.WriteAllLines(usersFilePath, lines);
                System.Diagnostics.Debug.WriteLine($"[UserService] Saved {users.Count} users to {usersFilePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UserService] Error saving users: {ex.Message}");
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
            user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            SaveUsersToFile();
            System.Diagnostics.Debug.WriteLine($"[UserService] Added new user: {user.Username}, Role: {user.Role}");
        }

        public static User GetUser(string username)
        {
            var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            System.Diagnostics.Debug.WriteLine($"[UserService] GetUser called for: {username}. Found: {user != null}");
            return user;
        }

        public static bool ValidateUser(string username, string password)
        {
            System.Diagnostics.Debug.WriteLine("=== [UserService] Checking Credentials ===");
            System.Diagnostics.Debug.WriteLine($"Entered Username: {username}");
            System.Diagnostics.Debug.WriteLine($"Entered Password: {password}");

            foreach (var user in users)
            {
                System.Diagnostics.Debug.WriteLine($"Stored User -> Username: {user.Username}, Password: {user.Password}, Role: {user.Role}");
            }

            bool result = users.Any(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            System.Diagnostics.Debug.WriteLine($"Authentication Result: {result}");
            return result;
        }

        // New method to check if user is staff
        public static bool IsStaff(string username)
        {
            var user = GetUser(username);
            bool isStaff = user?.Role == "Staff";
            System.Diagnostics.Debug.WriteLine($"[UserService] IsStaff('{username}') -> {isStaff}");
            return isStaff;
        }
    }
}
