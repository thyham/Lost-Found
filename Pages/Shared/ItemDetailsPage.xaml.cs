namespace MauiApp3
{
    public partial class ItemDetailsPage : ContentPage
    {
        private Item _item;
        private ItemsViewModel _itemsViewModel;

        public ItemDetailsPage(Item item, ItemsViewModel itemsViewModel)
        {
            InitializeComponent();
      
            var freshItem = ItemService.GetItem(item.Id);
            _item = freshItem ?? item;

            _itemsViewModel = itemsViewModel;
            BindingContext = _item;

            LoadItemImage();
        }

        private void LoadItemImage()
        {
            try
            {
                if (_item != null)
                {
                    if (!string.IsNullOrEmpty(_item.ImagePath))
                    {
                        if (File.Exists(_item.ImagePath))
                        {
                            ItemImageDisplay.Source = ImageSource.FromFile(_item.ImagePath);
                            ItemImageDisplay.IsVisible = true;
                            NoImageLabel.IsVisible = false;
                        }
                        else
                        {
                            ItemImageDisplay.IsVisible = false;
                            NoImageLabel.IsVisible = true;
                        }
                    }
                    else
                    {
                        ItemImageDisplay.IsVisible = false;
                        NoImageLabel.IsVisible = true;
                    }
                }
                else
                {
                    ItemImageDisplay.IsVisible = false;
                    NoImageLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                ItemImageDisplay.IsVisible = false;
                NoImageLabel.IsVisible = true;
            }
        }

        private async void OnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}