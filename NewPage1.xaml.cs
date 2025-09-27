namespace MauiApp3;

[QueryProperty(nameof(Item), "ItemPass")]
public partial class NewPage1 : ContentPage
{
    private Item item;
    public NewPage1(Item selectedItem)
	{
        InitializeComponent();
        BindingContext = selectedItem;
	}
}