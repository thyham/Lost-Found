using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace MauiApp3
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        int count1 = 0;
        private bool isButtonPressed = false;
        public ObservableCollection<Item> ItemsCollection { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ItemsViewModel();
        }


        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void OnGoToDetailsClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button?.BindingContext is Item selectedItem)
            {
                await Navigation.PushAsync(new NewPage1(selectedItem));
            }
        }




        //private void OnCounterClicked1(object? sender, EventArgs e)
        //{
        //    count1++;
        //    if (count1 > 10)
        //    {
        //        CounterBtn1.Text = $"Clicked {count1} time";
        //        count1 += 10;
        //    }
        //    else
        //        CounterBtn1.Text = $"Clicked {count1} times";

        //    SemanticScreenReader.Announce(CounterBtn1.Text);
        //}


        // When request is clicked, item status is changed and new button is generated
        private void MyButtonClicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            isButtonPressed = !isButtonPressed; // Toggle the state

            if (isButtonPressed && clickedButton?.BindingContext is Item selectedItem) 
            {
                selectedItem.Status = "Changed";
                clickedButton.BackgroundColor = Colors.Grey;
                clickedButton.Text = "Pending Request";
                clickedButton.TextColor = Colors.White;
                clickedButton.FontAttributes = FontAttributes.Bold;
                clickedButton.FontSize = 12;
            }
        }




    }
}
