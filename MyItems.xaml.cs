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

            // Default view: show all requested forms
            viewModel.FilterRequestedForms();
            FormsList.ItemsSource = viewModel.RequestedForms;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh data when page appears to show staff responses
            viewModel.RefreshFromService();

            // Re-apply the current filter
            if (activeButton == ApprovedFormsTabButton)
            {
                viewModel.FilterApprovedForms();
                FormsList.ItemsSource = viewModel.ApprovedForms;
            }
            else if (activeButton == RejectedFormsTabButton)
            {
                viewModel.FilterRejectedForms();
                FormsList.ItemsSource = viewModel.RejectedForms;
            }
            else
            {
                viewModel.FilterRequestedForms();
                FormsList.ItemsSource = viewModel.RequestedForms;
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
            // If clicking the same active button again, unselect and go back to RequestedForms
            if (activeButton == clickedButton)
            {
                ResetButtons();
                viewModel.FilterRequestedForms();
                FormsList.ItemsSource = viewModel.RequestedForms;
                activeButton = null;
                return;
            }

            // Set new active button (highlight) and reset the other
            SetActiveButton(clickedButton);
            activeButton = clickedButton;

            // Filter and update based on the selected tab
            switch (filterType)
            {
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

        private void SetActiveButton(Button active)
        {
            // Reset all button colors
            ApprovedFormsTabButton.BackgroundColor = Color.FromArgb("#6C757D");
            RejectedFormsTabButton.BackgroundColor = Color.FromArgb("#6C757D");

            // Highlight active one
            active.BackgroundColor = Color.FromArgb("#007BFF");
        }

        private void ResetButtons()
        {
            ApprovedFormsTabButton.BackgroundColor = Color.FromArgb("#6C757D");
            RejectedFormsTabButton.BackgroundColor = Color.FromArgb("#6C757D");
        }
    }
}