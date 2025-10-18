using System.Collections.ObjectModel;

namespace MauiApp3
{
    public partial class MyItems : ContentPage
    {
        private FormViewModel viewModel;
        private Button? activeButton = null;

        public MyItems()
        {
            InitializeComponent();
            viewModel = ViewModelLocator.FormVM;
            BindingContext = viewModel;

            // Default view: show all forms
            activeButton = AllFormsTabButton;
            ShowAllForms();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh data when page appears to show staff responses
            viewModel.RefreshFromService();

            // Re-apply the current filter
            if (activeButton == AllFormsTabButton)
            {
                ShowAllForms();
            }
            else if (activeButton == PendingFormsTabButton)
            {
                viewModel.FilterRequestedForms();
                FormsList.ItemsSource = viewModel.RequestedForms;
            }
            else if (activeButton == ApprovedFormsTabButton)
            {
                viewModel.FilterApprovedForms();
                FormsList.ItemsSource = viewModel.ApprovedForms;
            }
            else if (activeButton == RejectedFormsTabButton)
            {
                viewModel.FilterRejectedForms();
                FormsList.ItemsSource = viewModel.RejectedForms;
            }
        }

        private async void OnViewRequestDetailsClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button?.BindingContext is Form selectedForm)
            {
                if (selectedForm.Status == "Pending")
                {
                    await Navigation.PushAsync(new StudentRequestDetailsPage(selectedForm));
                }
                else
                {
                    await Navigation.PushAsync(new RequestDetailsPage(selectedForm));
                }
            }
        }

        private void OnAllFormsTabClicked(object sender, EventArgs e)
        {
            HandleTabButtonClick(AllFormsTabButton, "All");
        }

        private void OnPendingFormsTabClicked(object sender, EventArgs e)
        {
            HandleTabButtonClick(PendingFormsTabButton, "Pending");
        }

        private void OnApprovedFormsTabClicked(object sender, EventArgs e)
        {
            HandleTabButtonClick(ApprovedFormsTabButton, "Approved");
        }

        private void OnRejectedFormsTabClicked(object sender, EventArgs e)
        {
            HandleTabButtonClick(RejectedFormsTabButton, "Rejected");
        }

        private void HandleTabButtonClick(Button clickedButton, string filterType)
        {
            // If clicking the same active button, do nothing
            if (activeButton == clickedButton)
            {
                return;
            }

            // Set new active button (highlight)
            SetActiveButton(clickedButton);
            activeButton = clickedButton;

            // Filter and update based on the selected tab
            switch (filterType)
            {
                case "All":
                    ShowAllForms();
                    break;

                case "Pending":
                    viewModel.FilterRequestedForms();
                    FormsList.ItemsSource = viewModel.RequestedForms;
                    break;

                case "Approved":
                    viewModel.FilterApprovedForms();
                    FormsList.ItemsSource = viewModel.ApprovedForms;
                    break;

                case "Rejected":
                    viewModel.FilterRejectedForms();
                    FormsList.ItemsSource = viewModel.RejectedForms;
                    break;
            }
        }

        private void ShowAllForms()
        {
            // Filter all forms for current student
            var currentId = Preferences.Get("CurrentId", -1);
            var allUserForms = viewModel.FormsCollection
                .Where(form => form.studentId == currentId)
                .ToList();

            // Create a temporary observable collection
            var tempCollection = new ObservableCollection<Form>(allUserForms);
            FormsList.ItemsSource = tempCollection;
        }

        private void SetActiveButton(Button active)
        {
            // Reset all button colors
            AllFormsTabButton.BackgroundColor = Color.FromArgb("#6C757D");
            PendingFormsTabButton.BackgroundColor = Color.FromArgb("#6C757D");
            ApprovedFormsTabButton.BackgroundColor = Color.FromArgb("#6C757D");
            RejectedFormsTabButton.BackgroundColor = Color.FromArgb("#6C757D");

            // Highlight active one
            active.BackgroundColor = Color.FromArgb("#007BFF");
        }
    }
}