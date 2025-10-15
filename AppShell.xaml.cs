using Microsoft.Maui.Controls;
using System.Linq;

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
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(StaffPage), typeof(StaffPage));
            Routing.RegisterRoute(nameof(AddItemPage), typeof(AddItemPage));
            Routing.RegisterRoute(nameof(ItemDetailsPage), typeof(ItemDetailsPage));
            Routing.RegisterRoute(nameof(RequestDetailsPage), typeof(RequestDetailsPage));

            Loaded += AppShell_Loaded;
        }

        private async void AppShell_Loaded(object sender, EventArgs e)
        {
            // Redirect user to LoginPage if not logged in
            if (!Preferences.Get("IsLoggedIn", false))
            {
                await GoToAsync("//LoginPage");
            }
            else
            {
                // Show/hide flyout items based on user role
                UpdateFlyoutVisibility();
        }
        }

        private void UpdateFlyoutVisibility()
        {
            bool isStaff = Preferences.Get("IsStaff", false);

            // Get all ShellContent items from the Shell
            var allContent = this.Items.OfType<ShellContent>();

            foreach (var content in allContent)
            {
                // Hide student pages for staff and vice versa
                if (content.Route == "MainPage" || content.Route == "MyItems")
                {
                    content.FlyoutItemIsVisible = !isStaff;
                }
                else if (content.Route == "StaffPage")
                {
                    content.FlyoutItemIsVisible = isStaff;
                }
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
                FlyoutIsPresented = false;

                Preferences.Remove("IsLoggedIn");
                Preferences.Remove("CurrentUser");
                Preferences.Remove("IsStaff");

                // Navigate back to login page and clear navigation stack
                await GoToAsync("//LoginPage");
            }
        }
    }
}
