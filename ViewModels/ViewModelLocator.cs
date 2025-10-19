namespace MauiApp3
{
    public static class ViewModelLocator
    {
        // Creates instance of FormViewModel and ItemsViewModel to be used across the application and ensures data consistency
        public static FormViewModel FormVM { get; } = new FormViewModel();
        public static ItemsViewModel ItemsVM { get; } = new ItemsViewModel();
        public static Item SelectedItem { get; set; }
    }
}
