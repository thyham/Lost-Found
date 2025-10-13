using Microsoft.Maui.Controls;

namespace MauiApp3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));
            Routing.RegisterRoute(nameof(NewPage2), typeof(NewPage2));
            Routing.RegisterRoute(nameof(MyItems), typeof(MyItems));

            Loaded += AppShell_Loaded;
        }

        private async void AppShell_Loaded(object sender, EventArgs e)
        {
            // Redirect user to LoginPage if not logged in
            if (!Preferences.Get("IsLoggedIn", false))
            {
                await GoToAsync("//LoginPage");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Logout",
                "Are you sure you want to log out?",
                "Yes",
                "No");

            if (confirm)
            {
                Preferences.Remove("IsLoggedIn");
                Preferences.Remove("CurrentUser");

                // Navigate back to login page and clear navigation stack
                await GoToAsync("//LoginPage");
            }
        }
    }
}
