using Acr.UserDialogs;
using System;

namespace LinaqStorage.Helpers
{
    public static class UserDialogsService
    {
        public static void DisplaErrorMessage(string Message)
        {
            UserDialogs.Instance.Alert(Message,"Error","Ok");
        }

        public static void DisplayException(Exception e)
        {
            UserDialogs.Instance.Alert($"{e.Message}. InnerException: {e.InnerException}. StackTrace: {e.StackTrace}", "Error", "Ok");
        }

        public static void ShowAltert(string Message,string Title)
        {
            UserDialogs.Instance.Alert(Message, Title, "Ok");
        } 

        public static void ShowLoading(string Message)
        {
            UserDialogs.Instance.ShowLoading(Message);
        }
        public static void HideLoading()
        {
            UserDialogs.Instance.HideLoading();
        }
    }
}
