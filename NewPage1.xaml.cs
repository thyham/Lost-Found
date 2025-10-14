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
            BindingContext = _item; // Bind the current item
        }
    }

    public NewPage1()
    {
        InitializeComponent();
    }

    private async void OnGoToRequestClicked(object sender, EventArgs e)
    {
        if (_item != null)
        {
            // Store globally instead of passing through Shell route
            ViewModelLocator.SelectedItem = _item;
            await Shell.Current.GoToAsync(nameof(NewPage2));
        }
        else
        {
            await DisplayAlert("Error", "No item data found.", "OK");
        }
    }
}
