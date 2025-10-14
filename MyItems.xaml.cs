using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace MauiApp3
{
    public partial class MyItems : ContentPage
    {
        private bool isButtonPressed = false;
        private FormViewModel viewModel;
        private bool isShowingPending = false;
        private bool isShowingConfirmed = false;
        public ObservableCollection<Item> FormsCollection { get; set; }

        public MyItems()
        {
            InitializeComponent();
            viewModel = ViewModelLocator.FormVM;
            BindingContext = viewModel;
        }


        private void PendingButtonClicked(object sender, EventArgs e)
        {
            isShowingPending = !isShowingPending;

            if (isShowingPending)
            {
                viewModel.FilterRequestedForms();
                FormsList.ItemsSource = viewModel.RequestedForms;
            }
            else
            {
                FormsList.ItemsSource = viewModel.FormsCollection;
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


    }
}