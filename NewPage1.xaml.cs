namespace MauiApp3;

[QueryProperty(nameof(Item), "ItemPass")]
public partial class NewPage1 : ContentPage
{
    public NewPage1(Item selectedItem)
	{
        InitializeComponent();
        BindingContext = selectedItem;
	}

    private async void OnGoToRequestClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button?.BindingContext is Item selectedItem)
        {
            await Navigation.PushAsync(new NewPage2(selectedItem));
        }
    }
}