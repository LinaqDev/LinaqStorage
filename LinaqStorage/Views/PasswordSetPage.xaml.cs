using LinaqStorage.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinaqStorage.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordSetPage : ContentPage
    {
        public PasswordSetPage()
        {
            InitializeComponent();
            BindingContext = new PasswordSetViewModel();
        }
        public PasswordSetPage(string str)
        {
            InitializeComponent();
            BindingContext = new PasswordSetViewModel();
        }

        private void PasswordEntry_Completed(object sender, EventArgs e)
        {
            confirmPasswordEntry.Focus();
        }
    }
}