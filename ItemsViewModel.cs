using System.Collections.ObjectModel;

namespace MauiApp3
{
    public class ItemsViewModel
    {
        public ObservableCollection<Item> ItemsCollection { get; set; }

        //Keeping the collection data separate
        public ItemsViewModel()
        {
            ItemsCollection = new ObservableCollection<Item>
            {
                new Item { Name = "IPhone Charger", Category = "Technology", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a charger", Status = "Pending" },
                new Item { Name = "Black Jacket", Category = "Clothing", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is black", Status = "Open" },
                new Item { Name = "IPhone Charger", Category = "Technology", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a charger", Status = "Pending" },
                new Item { Name = "Black Jacket", Category = "Clothing", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is black", Status = "Open" },
                new Item { Name = "IPhone Charger", Category = "Technology", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a charger", Status = "Pending" },
                new Item { Name = "Black Jacket", Category = "Clothing", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is black", Status = "Open" }
            };
        }
    }
}
