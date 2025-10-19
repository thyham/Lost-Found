namespace MauiApp3
{
    public partial class RequestDetailsPage : ContentPage
    {
        private Form _form;

        public RequestDetailsPage(Form form)
        {
            InitializeComponent();
            _form = form;
            BindingContext = _form;

            LoadItemImage();
            DisplayCollectionInstructions();
        }

        private void LoadItemImage()
        {
            try
            {
                var item = ItemService.GetItem(_form.itemId);

                if (item != null)
                {
                    if (!string.IsNullOrEmpty(item.ImagePath) && File.Exists(item.ImagePath))
                    {
                        ItemImage.Source = ImageSource.FromFile(item.ImagePath);
                        NoImageLabel.IsVisible = false;
                    }
                    else
                    {
                        ItemImage.Source = null;
                        NoImageLabel.IsVisible = true;
                    }
                }
                else
                {
                    ItemImage.Source = null;
                    NoImageLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                ItemImage.Source = null;
                NoImageLabel.IsVisible = true;
            }
        }

        private void DisplayCollectionInstructions()
        {
            if (_form.Status == "Approved" && !string.IsNullOrWhiteSpace(_form.CollectionInstructions))
            {
                InstructionsBorder.IsVisible = true;
                InstructionsLabel.Text = _form.CollectionInstructions;
            }
            else
            {
                InstructionsBorder.IsVisible = false;
            }
        }

        private async void OnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}