using LinaqStorage.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinaqStorage.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginPageViewModel();
        }
        public LoginPage(string str)
        {
            InitializeComponent();
            BindingContext = new LoginPageViewModel();
        }
    }
}