namespace MauiApp3
{
    public partial class NewPage2 : ContentPage
    {
        private FormViewModel formViewModel = ViewModelLocator.FormVM;
        private Item _item;

        public Item ItemPass
        {
            get => _item;
            set
            {
                _item = value;
                BindingContext = _item;
            }
        }

        public NewPage2()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Always fetch the latest selected item
            _item = ViewModelLocator.SelectedItem;
            BindingContext = _item;
            StudentIdEntry.Text = Preferences.Get("CurrentId", -1).ToString();
        }

        public string ItemName => _item.Name;

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            var reasonText = DescriptionEditor.Text;

            if (string.IsNullOrWhiteSpace(reasonText))
            {
                await DisplayAlert("Error", "Please enter a description before submitting.", "OK");
                return;
            }

            var newForm = new Form
            {
                studentId = Preferences.Get("CurrentId", -1),
                itemId = _item.Id,
                itemName = _item.Name,
                Notes = reasonText,
                Status = "Pending"
            };

            // Save to service and refresh viewmodel
            FormService.AddForm(newForm);
            formViewModel.RefreshFromService();
            formViewModel.FilterRequestedForms();

            await DisplayAlert("Submitted", "Your claim request has been submitted and sent to staff!", "OK");
            await Navigation.PopAsync();
        }
    }
}