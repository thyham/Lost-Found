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

            if (!Preferences.Get("IsLoggedIn", false))
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                });
            }
            else
            {
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