using System.Collections.ObjectModel;

namespace MauiApp3
{
    public partial class MainPage : ContentPage
    {
        private ItemsViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = ViewModelLocator.ItemsVM;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.RefreshFromService();
        }

        private async void OnGoToDetailsClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button?.BindingContext is Item selectedItem)
            {
                await Shell.Current.GoToAsync($"{nameof(NewPage1)}", true,
                    new Dictionary<string, object> { { "ItemPass", selectedItem } });
            }
        }
    }
}