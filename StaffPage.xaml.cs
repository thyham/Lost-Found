namespace MauiApp3
{
    public partial class StaffPage : ContentPage
    {
        private ItemsViewModel itemsViewModel;
        private FormViewModel formViewModel;

        public StaffPage()
        {
            InitializeComponent();

            itemsViewModel = ViewModelLocator.ItemsVM;
            formViewModel = ViewModelLocator.FormVM;

            ItemsCollectionView.BindingContext = itemsViewModel;
            RequestsCollectionView.BindingContext = formViewModel;

            formViewModel.StaffFilterRequestedForms();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh data when page appears
            itemsViewModel.RefreshFromService();
            formViewModel.RefreshFromService();
            formViewModel.StaffFilterRequestedForms();
        }

        private void OnItemsTabClicked(object sender, EventArgs e)
        {
            ItemsSection.IsVisible = true;
            RequestsSection.IsVisible = false;

            ItemsTabButton.BackgroundColor = Color.FromArgb("#007AFF");
            RequestsTabButton.BackgroundColor = Color.FromArgb("#6C757D");
        }

        private void OnRequestsTabClicked(object sender, EventArgs e)
        {
            ItemsSection.IsVisible = false;
            RequestsSection.IsVisible = true;

            ItemsTabButton.BackgroundColor = Color.FromArgb("#6C757D");
            RequestsTabButton.BackgroundColor = Color.FromArgb("#007AFF");

            formViewModel.StaffFilterRequestedForms();
        }

        // Item Management
        private async void OnAddItemClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddItemPage(itemsViewModel));
        }

        private async void OnViewItemDetailsClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is Item selectedItem)
            {
                await Navigation.PushAsync(new ItemDetailsPage(selectedItem, itemsViewModel));
            }
        }

        private async void OnDeleteItemClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is Item selectedItem)
            {
                bool confirm = await DisplayAlert(
                    "Delete Item",
                    $"Are you sure you want to delete '{selectedItem.Name}'?",
                    "Delete",
                    "Cancel");

                if (confirm)
                {
                    ItemService.DeleteItem(selectedItem.Id);
                    itemsViewModel.RefreshFromService();
                    await DisplayAlert("Success", "Item deleted successfully", "OK");
                }
            }
        }

        // Request Management
        private async void OnViewRequestDetailsClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is Form selectedForm)
            {
                await Navigation.PushAsync(new RequestDetailsPage(selectedForm));
            }
        }

        private async void OnAcceptRequestClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is Form selectedForm)
            {
                bool confirm = await DisplayAlert(
                    "Accept Request",
                    $"Accept claim request for '{selectedForm.itemName}' by Student ID {selectedForm.studentId}?",
                    "Accept",
                    "Cancel");

                if (confirm)
                {
                    selectedForm.Status = "Approved";
                    FormService.UpdateForm(selectedForm);
                    formViewModel.RefreshFromService();
                    formViewModel.StaffFilterRequestedForms();
                    await DisplayAlert("Success", "Request approved!", "OK");
                }
            }
        }

        private async void OnRejectRequestClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is Form selectedForm)
            {
                bool confirm = await DisplayAlert(
                    "Reject Request",
                    $"Reject claim request for '{selectedForm.itemName}'?",
                    "Reject",
                    "Cancel");

                if (confirm)
                {
                    selectedForm.Status = "Rejected";
                    FormService.UpdateForm(selectedForm);
                    formViewModel.RefreshFromService();
                    formViewModel.StaffFilterRequestedForms();
                    await DisplayAlert("Rejected", "Request has been rejected", "OK");
                }
            }
        }
    }
}