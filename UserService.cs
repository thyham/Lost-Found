using System.Collections.Generic;
using System.Linq;

namespace MauiApp3
{
    public static class UserService
    {
        private static List<User> users = new List<User>
        {
            new User { Username = "admin", Email = "admin@example.com", Password = "admin123" },
            new User { Username = "demo", Email = "demo@example.com", Password = "demo123" }
        };

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
    }
}