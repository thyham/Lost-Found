namespace MauiApp3
{
    public static class ViewModelLocator
    {
        public static FormViewModel FormVM { get; } = new FormViewModel();
        public static Item SelectedItem { get; set; }
    }
}
