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
            BindingContext = _item; // Now binds your page to this specific item
        }
    }

    public NewPage1()
    {
        InitializeComponent();
    }

    private async void OnGoToRequestClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (_item != null)
        {
            // Navigate to the next page and pass the same item
            await Shell.Current.GoToAsync($"{nameof(NewPage2)}", true,
    new Dictionary<string, object> { { "ItemPass", _item } });

        }
        else
        {
            await DisplayAlert("Error", "No item data found.", "OK");
        }
    }
}
