namespace MauiApp3
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            var usersFilePath = Path.Combine(FileSystem.AppDataDirectory, "users.txt");
            if (File.Exists(usersFilePath))
            {
                File.Delete(usersFilePath);
                // Force UserService to reinitialize
                var users = UserService.GetUsers();
            }
            CheckRememberedUser();
        }

        private void CheckRememberedUser()
        {
            if (Preferences.ContainsKey("RememberMe") && Preferences.Get("RememberMe", false))
            {
                string savedUsername = Preferences.Get("SavedUsername", string.Empty);
                string savedPassword = Preferences.Get("SavedPassword", string.Empty);

                if (!string.IsNullOrEmpty(savedUsername) && !string.IsNullOrEmpty(savedPassword))
                {
                    UsernameEntry.Text = savedUsername;
                    PasswordEntry.Text = savedPassword;
                    RememberMeCheckBox.IsChecked = true;
                }
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Please enter both username and password");
                return;
            }

            //LoginButton.IsEnabled = false;
            LoginButton.Text = "Logging in...";

            bool isAuthenticated = await AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                // Save Remember Me preferences
                if (RememberMeCheckBox.IsChecked)
                {
                    Preferences.Set("RememberMe", true);
                    Preferences.Set("SavedUsername", username);
                    Preferences.Set("SavedPassword", password);
                }
                else
                {
                    Preferences.Remove("RememberMe");
                    Preferences.Remove("SavedUsername");
                    Preferences.Remove("SavedPassword");
                }

                // Save login status and current user
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("CurrentUser", username);

                // Check if user is staff or student
                bool isStaff = UserService.IsStaff(username);
                Preferences.Set("IsStaff", isStaff);

                // Navigate to appropriate page based on role
                if (isStaff)
                {
                    await Shell.Current.GoToAsync("//StaffPage");
                }
                else
                {
                await Shell.Current.GoToAsync("//MainPage");
                }
            }
            else
            {
                ShowError("Invalid username or password");
                LoginButton.IsEnabled = true;
                LoginButton.Text = "Login";
            }
        }

        private async Task<bool> AuthenticateUser(string username, string password)
        {
            await Task.Delay(1000); // Simulate network delay
            var users = UserService.GetUsers();
            return users.Any(u => u.Username == username && u.Password == password);
        }

        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}