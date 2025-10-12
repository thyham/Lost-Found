namespace MauiApp3;

[QueryProperty(nameof(ItemPass), "ItemPass")]
public partial class NewPage2 : ContentPage
{

    private Item _item;

    public Item ItemPass
    {
        get => _item;
        set
        {
            _item = value;
            BindingContext = _item;
        }
    }
    public NewPage2()
    {
        InitializeComponent();
    }
}