using System.Collections.Generic;
using System.Linq;

namespace MauiApp3
{
    public static class ItemService
    {
        private static List<Item> items = new List<Item>();
        private static readonly string itemsFilePath = Path.Combine(FileSystem.AppDataDirectory, "items.txt");

        static ItemService()
        {
            LoadItemsFromFile();
        }

        private static void LoadItemsFromFile()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[ItemService] Loading items from file: {itemsFilePath}");

                if (File.Exists(itemsFilePath))
                {
                    var lines = File.ReadAllLines(itemsFilePath);
                    System.Diagnostics.Debug.WriteLine($"[ItemService] Found {lines.Length} lines in items file");

                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var parts = line.Split('|');
                        if (parts.Length >= 7)
                        {
                            var item = new Item
                            {
                                Id = int.TryParse(parts[0], out var id) ? id : items.Count + 1,
                                Name = parts[1],
                                Category = parts[2],
                                Date = parts[3],
                                Location = parts[4],
                                Notes = parts[5],
                                Status = parts[6]
                            };
                            items.Add(item);
                            System.Diagnostics.Debug.WriteLine($"[ItemService] Loaded item: {item.Id}, Name: {item.Name}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[ItemService] No existing items file found. Creating default items...");
                    // Create default items
                    items.Add(new Item { Id = 1, Name = "IPhone Charger", Category = "Technology", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is an iphone 16 charger", Status = "Pending" });
                    items.Add(new Item { Id = 2, Name = "Black Jacket", Category = "Clothing", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a puffer jacket", Status = "Confirmed" });
                    items.Add(new Item { Id = 3, Name = "Blue Jacket", Category = "Clothing", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a tommy hilfiger jacket", Status = "Pending" });
                    items.Add(new Item { Id = 4, Name = "Samsung Charger", Category = "Technology", Date = "08/12/2025", Location = "Building 11.5.204", Notes = "It is a samsung 21fe charger", Status = "Pending" });
                    SaveItemsToFile();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ItemService] Error loading items: {ex.Message}");
            }
        }

        private static void SaveItemsToFile()
        {
            try
            {
                var lines = items.Select(i =>
                    $"{i.Id}|{i.Name}|{i.Category}|{i.Date}|{i.Location}|{i.Notes}|{i.Status}");
                File.WriteAllLines(itemsFilePath, lines);
                System.Diagnostics.Debug.WriteLine($"[ItemService] Saved {items.Count} items to {itemsFilePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ItemService] Error saving items: {ex.Message}");
            }
        }

        public static List<Item> GetItems()
        {
            return new List<Item>(items);
        }

        public static void AddItem(Item item)
        {
            item.Id = items.Count > 0 ? items.Max(i => i.Id) + 1 : 1;
            items.Add(item);
            SaveItemsToFile();
            System.Diagnostics.Debug.WriteLine($"[ItemService] Added new item: {item.Id}, Name: {item.Name}");
        }

        public static void UpdateItem(Item item)
        {
            var existingItem = items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Name = item.Name;
                existingItem.Category = item.Category;
                existingItem.Date = item.Date;
                existingItem.Location = item.Location;
                existingItem.Notes = item.Notes;
                existingItem.Status = item.Status;
                SaveItemsToFile();
                System.Diagnostics.Debug.WriteLine($"[ItemService] Updated item: {item.Id}");
            }
        }

        public static void DeleteItem(int itemId)
        {
            var item = items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                items.Remove(item);
                SaveItemsToFile();
                System.Diagnostics.Debug.WriteLine($"[ItemService] Deleted item: {itemId}");
            }
        }

        public static Item GetItem(int itemId)
        {
            return items.FirstOrDefault(i => i.Id == itemId);
        }
    }
}