using LinaqStorage.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinaqStorage.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordChangePage : ContentPage
    {
        public PasswordChangePage()
        {
            InitializeComponent();
            BindingContext = new PasswordChangeViewModel();
        }
        public PasswordChangePage(string str)
        {
            InitializeComponent();
            BindingContext = new PasswordChangeViewModel();
        }

        private void PasswordEntry_Completed(object sender, EventArgs e)
        {
            confirmPasswordEntry.Focus();
        }

        private void OldpasswordEntry_Completed(object sender, EventArgs e)
        {
            passwordEntry.Focus();
        }
    }
}