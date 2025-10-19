using System.Collections.ObjectModel;

namespace MauiApp3;

[QueryProperty(nameof(ItemPass), "ItemPass")]
public partial class NewPage1 : ContentPage
{
    private Item _item;

    public Item ItemPass
    {
        get => _item;
        set
        {
            _item = value;

            if (_item != null)
            {
                var freshItem = ItemService.GetItem(_item.Id);
                if (freshItem != null)
                {
                    _item = freshItem;
                }
            }

            BindingContext = _item;
            LoadItemImage();
        }
    }

    public NewPage1()
    {
        InitializeComponent();
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

    private async void OnGoToRequestClicked(object sender, EventArgs e)
    {
        if (_item != null)
        {
            ViewModelLocator.SelectedItem = _item;
            await Shell.Current.GoToAsync(nameof(NewPage2));
        }
        else
        {
            await DisplayAlert("Error", "No item data found.", "OK");
        }
    }
}