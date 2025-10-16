namespace MauiApp3
{
    public partial class StudentRequestDetailsPage : ContentPage
    {
        private Form _form;
        private FormViewModel viewModel;

        public StudentRequestDetailsPage(Form form)
        {
            InitializeComponent();
            viewModel = ViewModelLocator.FormVM;
            _form = form;
            BindingContext = _form;
        }

        private async void OnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnCancelRequestClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is Form selectedForm)
            {
                bool confirm = await DisplayAlert(
                    "Cancel Request",
                    $"Cancel claim request for '{selectedForm.itemName}'?",
                    "Yes",
                    "No");

                if (confirm)
                {
                    selectedForm.Status = "Pending";
                    viewModel.FilterRequestedForms();
                    await DisplayAlert("Cancelled", "Request has been cancelled", "OK");
                }
            }
        }
    }
}