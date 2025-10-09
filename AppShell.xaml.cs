namespace MauiApp3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            
            // If user is not logged in, navigate to login
            if (!Preferences.Get("IsLoggedIn", false))
            {
                Current.GoToAsync("///LoginPage");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Logout",
                "Are you sure you want to logout?",
                "Yes",
                "No");

            if (confirm)
            {
                Preferences.Remove("IsLoggedIn");
                Preferences.Remove("CurrentUser");
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}