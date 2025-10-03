using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace MauiApp3
{
    public class ItemsViewModel
    {
        public ObservableCollection<Item> ItemsCollection { get; set; }
        public ObservableCollection<Item> FilteredItems { get; set; }


        //Keeping the collection data separate
        public ItemsViewModel()
        {
            ItemsCollection = new ObservableCollection<Item>
            {
                new Item { Name = "IPhone Charger", Category = "Technology", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is an iphone 16 charger", Status = "Pending" },
                new Item { Name = "Black Jacket", Category = "Clothing", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a puffer jacket", Status = "Pending" },
                new Item { Name = "Blue Jacket", Category = "Clothing", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a tommy hilfiger jacket", Status = "Pending" },
                new Item { Name = "Samsung Charger", Category = "Technology", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a samsung 21fe charger", Status = "Pending" },
            };

            FilteredItems = new ObservableCollection<Item>();
        }

        private void FilterItems(string searchText)
        {
            FilteredItems.Clear();

            IEnumerable<Item> filtered;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                filtered = Enumerable.Empty<Item>();
            }
            else
            {
                FilteredItems.Clear();
                filtered = ItemsCollection.Where(item =>
                    item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    item.Notes.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var item in filtered)
            {
                FilteredItems.Add(item);
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
