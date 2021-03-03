using LinaqStorage.Interfaces;
using System;
using System.ComponentModel;

namespace LinaqStorage.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        internal INavigationService NavigationService { get; } = App.NavigationService; 

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void RaiseStaticPropertyChanged(string propName)
        {
            EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
            if (handler != null)
                handler(null, new PropertyChangedEventArgs(propName));

        }
    }
}
