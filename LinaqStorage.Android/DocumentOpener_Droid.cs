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

[assembly: Dependency(typeof(DocumentOpener_Droid))]
namespace LinaqStorage.Droid
{
    public class DocumentOpener_Droid : IDocumentOpener
    {
        public void OpenFile(string path)
        {
            try
            {
                Android.Net.Uri uri;

                string auth = "com.Linaq.LinaqStorage.Android.fileprovider";
                string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(path.ToLower()));
                if (mimeType == null)
                    mimeType = "*/*";

                if (path.StartsWith("content://"))
                {
                     uri = Android.Net.Uri.Parse(path);
                }
                else
                {
                    var file = new Java.IO.File(Path.Combine(Android.App.Application.Context.FilesDir.Path, path));
                     uri = FileProvider.GetUriForFile(Android.App.Application.Context, auth, file);

                }
                //Intent.ActionView)
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(uri, mimeType);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
                intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.NoHistory);
                //intent.SetAction(Intent.ActionGetContent);

                // Trying to allow writing to the external app ...
                var resInfoList = Android.App.Application.Context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
                foreach (var resolveInfo in resInfoList)
                {
                    var packageName = resolveInfo.ActivityInfo.PackageName;
                    Android.App.Application.Context.GrantUriPermission(packageName, uri, ActivityFlags.GrantWriteUriPermission | ActivityFlags.GrantPrefixUriPermission | ActivityFlags.GrantReadUriPermission);
                }

                Android.App.Application.Context.StartActivity(intent);
            }
            catch (Exception ex)
            {
                UserDialogsService.ShowAltert(TranslateService.ProvideValue("CantOpenFile"), "");
             //   Toast.MakeText(Android.App.Application.Context, TranslateService.ProvideValue("CantOpenFile"), ToastLength.Long).Show();
                // Toast.MakeText(Android.App.Application.Context, $"Exception {ex.Message}, InnerException: {ex.InnerException}. StackTrace: {ex.StackTrace}", ToastLength.Long).Show();
            }


        }


    }
}