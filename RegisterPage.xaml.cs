namespace MauiApp3
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Validation
            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Username is required");
                return;
            }

            if (username.Length < 3)
            {
                ShowError("Username must be at least 3 characters");
                return;
            }

            // Check for reserved staff username
            if (username.Equals("staff", StringComparison.OrdinalIgnoreCase))
            {
                ShowError("This username is reserved");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowError("Email is required");
                return;
            }

            if (!IsValidEmail(email))
            {
                ShowError("Please enter a valid email address");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Password is required");
                return;
            }

            if (password.Length < 6)
            {
                ShowError("Password must be at least 6 characters");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Passwords do not match");
                return;
            }

            if (UserService.UserExists(username))
            {
                ShowError("Username already exists");
                return;
            }

            // Disable button and show loading state
            RegisterButton.IsEnabled = false;
            RegisterButton.Text = "Creating account...";

            await Task.Delay(1000); // Simulate network delay

            // Always create as Student (role is enforced in UserService.AddUser)
            var newUser = new User
            {
                Username = username,
                Email = email,
                Password = password,
                Role = "Student"
            };

            UserService.AddUser(newUser);

            ShowSuccess("Student account created successfully!");

            await Task.Delay(1500);

            // Navigate back to login page
            await Navigation.PopAsync();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ShowError(string message)
        {
            MessageLabel.Text = message;
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.IsVisible = true;
        }

        private void ShowSuccess(string message)
        {
            MessageLabel.Text = message;
            MessageLabel.TextColor = Colors.Green;
            MessageLabel.IsVisible = true;
        }
    }
}