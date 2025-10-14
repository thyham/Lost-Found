namespace MauiApp3
{
    public partial class ItemDetailsPage : ContentPage
    {
        private Item _item;
        private ItemsViewModel _itemsViewModel;

        public ItemDetailsPage(Item item, ItemsViewModel itemsViewModel)
        {
            InitializeComponent();
            _item = item;
            _itemsViewModel = itemsViewModel;
            BindingContext = _item;
        }

        private async void OnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}