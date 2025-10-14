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
            string role = RolePicker.SelectedItem?.ToString() ?? "Student";

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

            RegisterButton.IsEnabled = false;
            RegisterButton.Text = "Creating account...";

            await Task.Delay(1000);

            var newUser = new User
            {
                Username = username,
                Email = email,
                Password = password,
                Role = role
            };

            UserService.AddUser(newUser);

            ShowSuccess($"Account created successfully as {role}!");

            await Task.Delay(1500);

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