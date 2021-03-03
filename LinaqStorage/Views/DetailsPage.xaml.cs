using LinaqStorage.Models;
using LinaqStorage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinaqStorage.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage()
        {
            InitializeComponent();
            BindingContext = new DetailsViewModel();
        }

        public DetailsPage(FileObject obj)
        {
            InitializeComponent();
            BindingContext = new DetailsViewModel(obj);
        }
    }
}