namespace MauiApp3
{
    public partial class AddItemPage : ContentPage
    {
        private ItemsViewModel _itemsViewModel;

        public AddItemPage(ItemsViewModel itemsViewModel)
        {
            InitializeComponent();
            _itemsViewModel = itemsViewModel;
            DatePicker.Date = DateTime.Now;
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
            int newId = _itemsViewModel.ItemsCollection.Count > 0
                ? _itemsViewModel.ItemsCollection.Max(i => i.Id) + 1
                : 1;

            var newItem = new Item
            {
                Id = newId,
                Name = name,
                Category = category,
                Location = location,
                Date = date,
                Notes = notes ?? "No additional notes",
                Status = "Pending"
            };

            _itemsViewModel.ItemsCollection.Add(newItem);

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