using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Support.V4.Provider;
using Android.Widget;
using LinaqStorage.Droid;
using LinaqStorage.Helpers;
using LinaqStorage.Interfaces;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Dialogs_Droid))]
namespace LinaqStorage.Droid
{
    public class Dialogs_Droid : IDialogs
    {
        public void ShowWaitDialog()
        {
            
        }
    }
}