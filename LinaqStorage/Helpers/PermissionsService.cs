using Acr.UserDialogs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace LinaqStorage.Helpers
{
    public static class PermissionsService
    {
        public static async Task<bool> GetStoragePermissionsAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        UserDialogs.Instance.Alert("Need External Storage Permissions.", "Permissions", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    if (results.ContainsKey(Permission.Storage))
                        status = results[Permission.Storage];
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    UserDialogs.Instance.Alert("Can not continue, try again.", "Permission Unknown", "OK");
                   
                } 
            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
            }
            return false;
        }
    }
}
