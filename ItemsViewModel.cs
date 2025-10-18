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
            ItemsCollection = new ObservableCollection<Item>();
            FilteredItems = new ObservableCollection<Item>();
            PendingItems = new ObservableCollection<Item>();
            ConfirmedItems = new ObservableCollection<Item>();

            LoadItemsFromService();
        }

        private void LoadItemsFromService()
        {
            var items = ItemService.GetItems();
            ItemsCollection.Clear();
            FilteredItems.Clear();

            foreach (var item in items)
            {
                ItemsCollection.Add(item);
                FilteredItems.Add(item);
            }
        }

        public void RefreshFromService()
        {
            LoadItemsFromService();
            FilterPendingItems();
            FilterConfirmedItems();
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}