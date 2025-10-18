namespace MauiApp3
{
    public partial class AddItemPage : ContentPage
    {
        private ItemsViewModel _itemsViewModel;
        private string _selectedImagePath;

        public AddItemPage(ItemsViewModel itemsViewModel)
        {
            InitializeComponent();
            _itemsViewModel = itemsViewModel;
            DatePicker.Date = DateTime.Now;
        }

        private async void OnTakePhotoClicked(object sender, EventArgs e)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        await LoadAndSavePhoto(photo);
                    }
                }
                else
                {
                    await DisplayAlert("Not Supported", "Camera is not available on this device", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to take photo: {ex.Message}", "OK");
            }
        }

        private async void OnPickPhotoClicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    await LoadAndSavePhoto(photo);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to pick photo: {ex.Message}", "OK");
            }
        }

        private async Task LoadAndSavePhoto(FileResult photo)
        {
            try
            {
                // Save photo to app data directory
                var fileName = $"item_{Guid.NewGuid()}.jpg";
                var localPath = Path.Combine(FileSystem.AppDataDirectory, "ItemImages", fileName);

                // Create directory if it doesn't exist
                var directory = Path.GetDirectoryName(localPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Copy the file
                using (var stream = await photo.OpenReadAsync())
                using (var fileStream = File.Create(localPath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                // Store the path and display the image
                _selectedImagePath = localPath;
                ItemImage.Source = ImageSource.FromFile(localPath);
                ItemImage.IsVisible = true;
                NoImageLabel.IsVisible = false;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save photo: {ex.Message}", "OK");
            }
        }

        private async void OnAddItemClicked(object sender, EventArgs e)
        {
            string name = NameEntry.Text?.Trim();
            string category = CategoryPicker.SelectedItem?.ToString();
            string location = LocationEntry.Text?.Trim();
            string notes = NotesEditor.Text?.Trim();
            string date = DatePicker.Date.ToString("dd/MM/yyyy");

            // Validation
            if (string.IsNullOrWhiteSpace(name))
            {
                ShowError("Please enter an item name");
                return;
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                ShowError("Please select a category");
                return;
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                ShowError("Please enter the location where item was found");
                return;
            }

            // Create new item
            var newItem = new Item
            {
                Name = name,
                Category = category,
                Location = location,
                Date = date,
                Notes = notes ?? "No additional notes",
                Status = "Pending",
                ImagePath = _selectedImagePath // Save the image path
            };

            // Save to service and refresh viewmodel
            ItemService.AddItem(newItem);
            _itemsViewModel.RefreshFromService();

            await DisplayAlert("Success", $"'{name}' has been added to lost items", "OK");
            await Navigation.PopAsync();
        }

        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        }
    }
}