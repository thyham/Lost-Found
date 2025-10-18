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
            Routing.RegisterRoute(nameof(StudentRequestDetailsPage), typeof(StudentRequestDetailsPage));

            Navigated += OnShellNavigated;
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

        private void OnShellNavigated(object sender, ShellNavigatedEventArgs e)
        {
            // Disable flyout on login and register pages
            var currentPage = CurrentPage?.GetType().Name;

            if (currentPage == nameof(LoginPage) || currentPage == nameof(RegisterPage))
            {
                FlyoutBehavior = FlyoutBehavior.Disabled;
            }
            else
            {
                FlyoutBehavior = FlyoutBehavior.Flyout;
                // Update visibility when navigating to ensure correct items show
                UpdateFlyoutVisibility();
            }
        }

        private void UpdateFlyoutVisibility()
        {
            bool isStaff = Preferences.Get("IsStaff", false);

            // Use x:Name references for direct access
            if (isStaff)
            {
                // Staff: Show only Staff Dashboard
                HomeShellContent.FlyoutItemIsVisible = false;
                MyItemsShellContent.FlyoutItemIsVisible = false;
                StaffPageShellContent.FlyoutItemIsVisible = true;
            }
            else
            {
                // Student: Show Home and My Items
                HomeShellContent.FlyoutItemIsVisible = true;
                MyItemsShellContent.FlyoutItemIsVisible = true;
                StaffPageShellContent.FlyoutItemIsVisible = false;
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