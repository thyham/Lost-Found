namespace MauiApp3
{
    public partial class StaffPage : ContentPage
    {
        private ItemsViewModel itemsViewModel;
        private FormViewModel formViewModel;
        private string currentRequestFilter = "All";

        public StaffPage()
        {
            InitializeComponent();

            itemsViewModel = ViewModelLocator.ItemsVM;
            formViewModel = ViewModelLocator.FormVM;

            ItemsCollectionView.BindingContext = itemsViewModel;
            RequestsCollectionView.BindingContext = formViewModel;

         
            formViewModel.StaffFilterAllRequests();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            itemsViewModel.RefreshFromService();
            formViewModel.RefreshFromService();

       
            ApplyRequestFilter(currentRequestFilter);
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

            ApplyRequestFilter(currentRequestFilter);

         
            if (currentRequestFilter == "All")
                SetActiveRequestButton(AllRequestsButton);
            else if (currentRequestFilter == "Pending")
                SetActiveRequestButton(PendingRequestsButton);
            else if (currentRequestFilter == "Approved")
                SetActiveRequestButton(AcceptedRequestsButton);
            else if (currentRequestFilter == "Rejected")
                SetActiveRequestButton(RejectedRequestsButton);
        }

       
        private void OnAllRequestsClicked(object sender, EventArgs e)
        {
            currentRequestFilter = "All";
            ApplyRequestFilter("All");
            SetActiveRequestButton(AllRequestsButton);
        }

        private void OnPendingRequestsClicked(object sender, EventArgs e)
        {
            currentRequestFilter = "Pending";
            ApplyRequestFilter("Pending");
            SetActiveRequestButton(PendingRequestsButton);
        }

        private void OnAcceptedRequestsClicked(object sender, EventArgs e)
        {
            currentRequestFilter = "Approved";
            ApplyRequestFilter("Approved");
            SetActiveRequestButton(AcceptedRequestsButton);
        }

        private void OnRejectedRequestsClicked(object sender, EventArgs e)
        {
            currentRequestFilter = "Rejected";
            ApplyRequestFilter("Rejected");
            SetActiveRequestButton(RejectedRequestsButton);
        }

        private void ApplyRequestFilter(string status)
        {
            if (status == "All")
            {
                formViewModel.StaffFilterAllRequests();
            }
            else
            {
                formViewModel.StaffFilterRequestsByStatus(status);
            }
        }

        private void SetActiveRequestButton(Button activeButton)
        {
            AllRequestsButton.BackgroundColor = Color.FromArgb("#6C757D");
            PendingRequestsButton.BackgroundColor = Color.FromArgb("#6C757D");
            AcceptedRequestsButton.BackgroundColor = Color.FromArgb("#6C757D");
            RejectedRequestsButton.BackgroundColor = Color.FromArgb("#6C757D");

            if (activeButton == AllRequestsButton)
                activeButton.BackgroundColor = Color.FromArgb("#007AFF");
            else if (activeButton == PendingRequestsButton)
                activeButton.BackgroundColor = Color.FromArgb("#FFC107");
            else if (activeButton == AcceptedRequestsButton)
                activeButton.BackgroundColor = Color.FromArgb("#28A745");
            else if (activeButton == RejectedRequestsButton)
                activeButton.BackgroundColor = Color.FromArgb("#DC3545");
        }

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
                
                string instructions = await DisplayPromptAsync(
                    "Collection Instructions",
                    "Enter instructions for the student on how to collect their item:",
                    placeholder: "e.g., Visit Building 11, Room 204, Mon-Fri 9AM-5PM",
                    maxLength: 500,
                    keyboard: Keyboard.Text);

                
                if (string.IsNullOrWhiteSpace(instructions))
                {
                    await DisplayAlert("Cancelled", "Request approval cancelled. Instructions are required.", "OK");
                    return;
                }

                bool confirm = await DisplayAlert(
                    "Accept Request",
                    $"Accept claim request for '{selectedForm.itemName}' by Student ID {selectedForm.studentId}?",
                    "Accept",
                    "Cancel");

                if (confirm)
                {
                    
                    selectedForm.Status = "Approved";
                    selectedForm.CollectionInstructions = instructions;
                    FormService.UpdateForm(selectedForm);

                    
                    var item = ItemService.GetItem(selectedForm.itemId);
                    if (item != null)
                    {
                        item.Status = "Confirmed";
                        ItemService.UpdateItem(item);
                    }

                    
                    formViewModel.RefreshFromService();
                    itemsViewModel.RefreshFromService();
                    ApplyRequestFilter(currentRequestFilter);

                    await DisplayAlert("Success",
                        $"Request approved! Student has been notified with collection instructions.",
                        "OK");
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
                    ApplyRequestFilter(currentRequestFilter);
                    await DisplayAlert("Rejected", "Request has been rejected", "OK");
                }
            }
        }
    }
}