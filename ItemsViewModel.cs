using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace MauiApp3
{
    public class ItemsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Item> ItemsCollection { get; set; }
        public ObservableCollection<Item> FilteredItems { get; set; }
        public ObservableCollection<Item> PendingItems { get; set; }
        public ObservableCollection<Item> ConfirmedItems { get; set; }

        private string _searchQuery;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    FilterItems(_searchQuery);
                }
            }
        }

        public ItemsViewModel()
        {
            // Original collection
            ItemsCollection = new ObservableCollection<Item>
            {
                new Item { Id = 1, Name = "IPhone Charger", Category = "Technology", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is an iphone 16 charger", Status = "Pending" },
                new Item { Id = 2, Name = "Black Jacket", Category = "Clothing", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a puffer jacket", Status = "Confirmed" },
                new Item { Id = 3, Name = "Blue Jacket", Category = "Clothing", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a tommy hilfiger jacket", Status = "Pending" },
                new Item { Id = 4,  Name = "Samsung Charger", Category = "Technology", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a samsung 21fe charger", Status = "Pending" },
            };

            // Initialize collections
            FilteredItems = new ObservableCollection<Item>(ItemsCollection); 
            PendingItems = new ObservableCollection<Item>();
            ConfirmedItems = new ObservableCollection<Item>();
        }

        public void FilterPendingItems()
        {
            PendingItems.Clear();
            var pending = ItemsCollection.Where(item =>
                item.Status.Contains("Pending", System.StringComparison.OrdinalIgnoreCase));
            foreach (var item in pending)
                PendingItems.Add(item);
        }

        public void FilterConfirmedItems()
        {
            ConfirmedItems.Clear();
            var confirmed = ItemsCollection.Where(item =>
                item.Status.Contains("Confirmed", System.StringComparison.OrdinalIgnoreCase));
            foreach (var item in confirmed)
                ConfirmedItems.Add(item);
        }

        private void FilterItems(string searchText)
        {
            FilteredItems.Clear();

            IEnumerable<Item> filtered;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Show all items when search is empty
                filtered = ItemsCollection;
            }
            else
            {
                filtered = ItemsCollection.Where(item =>
                    item.Name.Contains(searchText, System.StringComparison.OrdinalIgnoreCase) ||
                    item.Notes.Contains(searchText, System.StringComparison.OrdinalIgnoreCase));
            }

            foreach (var item in filtered)
            {
                FilteredItems.Add(item);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
