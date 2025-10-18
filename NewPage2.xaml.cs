
namespace MauiApp3
{
    public partial class NewPage2 : ContentPage
    {
        private FormViewModel formViewModel = ViewModelLocator.FormVM;
        private Item _item;

        private int _loggedInStudentId = 1234;

        public Item ItemPass
        {
            get => _item;
            set
            {
                _item = value;
                BindingContext = _item; // Bind the current item
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
        private void OnSubmitClicked(object sender, EventArgs e)
        {
            var reasonText = DescriptionEditor.Text;

            if (string.IsNullOrWhiteSpace(reasonText))
            {
                DisplayAlert("Error", "Please enter a description before submitting.", "OK");
                return;
            }

            var newForm = new Form
            {
                formId = formViewModel.FormsCollection.Count + 1,
                studentId = Preferences.Get("CurrentId", -1),
                itemId = _item.Id,
                itemName = _item.Name,
                Notes = reasonText,
                Status = "Pending"
            };

            formViewModel.FormsCollection.Add(newForm);
            formViewModel.FilterRequestedForms();

            DisplayAlert("Submitted", "Your claim request has been submitted!", "OK");
            Navigation.PopAsync();
        }
    }
}
