namespace MauiApp3
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            // Check login status BEFORE showing Shell UI
            if (!Preferences.Get("IsLoggedIn", false))
            {
                // Navigate to LoginPage as the starting route
                // ✅ Delay navigation until Shell is initialized
                Dispatcher.Dispatch(async () =>
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                });
            }
            else
            {
                // User is logged in, check their role and navigate accordingly
                bool isStaff = Preferences.Get("IsStaff", false);

                Dispatcher.Dispatch(async () =>
                {
                    if (isStaff)
                    {
                        await Shell.Current.GoToAsync("//StaffPage");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("//MainPage");
                    }
                });
            }

            return window;
        }
    }
}