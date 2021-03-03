using LinaqStorage.ViewModels;
using Xamarin.Forms;

namespace LinaqStorage
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
        public MainPage(string str)
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        } 
         

    }
}
