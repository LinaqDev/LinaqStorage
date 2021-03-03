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

[assembly: Dependency(typeof(DeleteFile_Droid))]
namespace LinaqStorage.Droid
{
    class DeleteFile_Droid : IDeleteFile
    {
        public void DeleteFile(string path)
        {
            Android.Net.Uri uri;

            string auth = "com.Linaq.LinaqStorage.Android.fileprovider";
            string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(path.ToLower()));
            if (mimeType == null)
                mimeType = "*/*";

            if (path.StartsWith("content://"))
            {
                uri = Android.Net.Uri.Parse(path);

                DocumentFile pickedDir = DocumentFile.FromSingleUri(Android.App.Application.Context, uri);
                if (pickedDir.Exists())
                {
                    pickedDir.Delete();

                }
            }
            else
            {
                var file = new Java.IO.File(Path.Combine(Android.App.Application.Context.FilesDir.Path, path));
                file.Delete();
              //  uri = FileProvider.GetUriForFile(Android.App.Application.Context, auth, file);

            }
             
        }
    }
}