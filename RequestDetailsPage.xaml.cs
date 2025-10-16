namespace MauiApp3
{
    public partial class RequestDetailsPage : ContentPage
    {
        private Form _form;

        public RequestDetailsPage(Form form)
        {
            InitializeComponent();
            _form = form;
            BindingContext = _form;
        }

        private async void OnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}