namespace MauiApp3;

[QueryProperty(nameof(Item), "ItemPass")]
public partial class NewPage2 : ContentPage
{
    public NewPage2(Item selectedItem)
    {
        InitializeComponent();
        BindingContext = selectedItem;
    }
}