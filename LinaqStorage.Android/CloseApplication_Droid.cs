using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Widget;
using LinaqStorage.Droid;
using LinaqStorage.Interfaces;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseApplication_Droid))]
namespace LinaqStorage.Droid
{
    public class CloseApplication_Droid : ICloseApplication
    {
        public void closeApplication()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}