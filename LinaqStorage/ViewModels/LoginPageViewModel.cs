using Acr.UserDialogs;
using LinaqStorage.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace LinaqStorage.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        public LoginPageViewModel()
        {
            LoginCmd = new RelayCommand(LoginExe);
            OpenChangePasswordPageCmd = new RelayCommand(OpenChangePasswordPageExe);
            AboutPageCmd = new RelayCommand(AboutPageExe);
            ResetPasswordCmd = new RelayCommand(ResetPasswordExe);
        }

        private async void OpenChangePasswordPageExe()
        {
            await NavigationService.NavigateAsync("PasswordChangePage", true);
        }

        private string _passwordEntry = "";
        public string PasswordEntry
        {
            get
            {
                return _passwordEntry;
            }
            set
            {
                _passwordEntry = value;
                RaisePropertyChanged("PasswordEntry");
            }
        }

        private async void LoginExe()
        {
            using (UserDialogs.Instance.Loading(TranslateService.ProvideValue("Loading")))
                if (CheckCredentials())
                {

                    //await Task.Delay(1500);
                    App.IsUserLoggedIn = true;
                    await NavigationService.InsertNewRootPage("MainPage");
                }
                else
                {
                    UserDialogsService.ShowAltert(TranslateService.ProvideValue("Message_WrongPassword"), TranslateService.ProvideValue("Message_LoginError"));
                }
        }
        private string UserPassword
        {
            get
            {
                return App.AppSettings.GetValueOrDefault(nameof(UserPassword), string.Empty);
            }
        }

        private bool CheckCredentials()
        {
            if (PasswordEntry == UserPassword)
                return true;
            else
                return false;
        }
        private async void AboutPageExe()
        {
            await NavigationService.NavigateAsync("AboutPage", true);
        }

        private async void ResetPasswordExe()
        {
            try
            {


                bool ConfirmResult = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Title = TranslateService.ProvideValue("PasswordPage_PasswordReset"),
                    Message = TranslateService.ProvideValue("PasswordPage_PasswordResetInfoLoseData"),
                    OkText = "OK",
                    CancelText = TranslateService.ProvideValue("Cancel")
                });

                if (ConfirmResult)
                {
                    ResetPassword();
                    await NavigationService.InsertNewRootPage("PasswordSetPage");
                }
               
            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
            }
        }

        public ICommand LoginCmd { get; set; }
        public ICommand OpenChangePasswordPageCmd { get; set; }
        public ICommand AboutPageCmd { get; set; }
        public ICommand ResetPasswordCmd { get; set; }

        private void ResetPassword()
        {
            try
            {
                JsonService.ResetList();
                App.AppSettings.AddOrUpdateValue("UserPassword", string.Empty);
                IEnumerable<string> files = Directory.EnumerateFiles(Constants.localAppDataFolder, "*");
                foreach (string f in files)
                {
                    File.Delete(f);
                }

                UserDialogs.Instance.Alert(TranslateService.ProvideValue("PasswordPage_PasswordResetSucessfully"), TranslateService.ProvideValue("PasswordPage_PasswordReset"), "Ok");

                
            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
            }

        }
    }
}
