namespace MauiApp3
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            CheckRememberedUser();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoginButton.IsEnabled = true;
            LoginButton.Text = "Login";

            ErrorLabel.IsVisible = false;
            ErrorLabel.Text = string.Empty;
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

            LoginButton.IsEnabled = false;
            LoginButton.Text = "Logging in...";

            bool isAuthenticated = UserService.ValidateUser(username, password);

            if (isAuthenticated)
            {
                var user = UserService.GetUser(username);

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

                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("CurrentId", user.Id);
                Preferences.Set("CurrentUser", username);

                bool isStaff = UserService.IsStaff(username);
                Preferences.Set("IsStaff", isStaff);

                System.Diagnostics.Debug.WriteLine($"[LoginPage] User '{username}' logged in. IsStaff: {isStaff}");

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


