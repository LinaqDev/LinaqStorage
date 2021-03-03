using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Widget;
using LinaqStorage.Droid;
using LinaqStorage.Helpers;
using LinaqStorage.Interfaces;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveFile_Droid))]
namespace LinaqStorage.Droid
{
    public class SaveFile_Droid:ISaveFile
    {
        public void SaveFile(byte[] file,string fileName)
        {
            var downloadDirectory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            var filePath = Path.Combine(downloadDirectory, fileName);
           
            var streamWriter = File.Create(filePath);
            streamWriter.Close();
            File.WriteAllBytes(filePath, file);
            UserDialogs.Instance.Toast($"Zapisano w: {filePath}");
        }
    }
}