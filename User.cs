namespace MauiApp3
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Student"; // "Student" or "Staff"
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}