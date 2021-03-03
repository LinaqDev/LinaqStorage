using LinaqStorage.Enums;
using LinaqStorage.Helpers;
using LinaqStorage.Helpers.CryptographyMethods;
using LinaqStorage.Resources;
using LinaqStorage.Views;
using Plugin.Multilingual;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LinaqStorage
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public static ViewNavigationService NavigationService { get; } = new ViewNavigationService();
        public static ISettings AppSettings => CrossSettings.Current;

        public App()
        {
            try
            {
                InitializeComponent();
                Constants.InitConstatns();
                InitCryptMethod();

                CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo(SelectedLanguage.ToLower());
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;

                NavigationService.Configure("MainPage", typeof(MainPage));
                NavigationService.Configure("DetailsPage", typeof(DetailsPage));
                NavigationService.Configure("SettingsPage", typeof(SettingsPage));
                NavigationService.Configure("LoginPage", typeof(LoginPage));
                NavigationService.Configure("PasswordChangePage", typeof(PasswordChangePage));
                NavigationService.Configure("PasswordSetPage", typeof(PasswordSetPage));
                NavigationService.Configure("AboutPage", typeof(AboutPage));
                NavigationService.Configure("HelpPage", typeof(HelpPage));

                if (!IsUserLoggedIn)
                {
                    if (!string.IsNullOrWhiteSpace(UserPassword))
                        MainPage = NavigationService.SetRootPage("LoginPage");
                    else
                        MainPage = NavigationService.SetRootPage("PasswordSetPage");
                }
                else
                {
                    MainPage = NavigationService.SetRootPage("MainPage");
                }
            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
            }
        }

        public string SelectedLanguage
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(SelectedLanguage), "en");
            }
        }

        private string UserPassword
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(UserPassword), string.Empty);
            }
        }

        private void InitCryptMethod()
        {
            string method = AppSettings.GetValueOrDefault("SelectedEncryptionMethod", string.Empty);
            if (string.IsNullOrWhiteSpace(method))
            {
                AppSettings.AddOrUpdateValue("SelectedEncryptionMethod", CryptMethod.AES.ToString());
            }
        }

        protected override void OnStart()
        {
            IEnumerable<string> files = Directory.EnumerateFiles(Constants.localAppDataFolder, "*", SearchOption.TopDirectoryOnly).Where(x => x.Contains("tmpDecrypted_"));
            foreach (string f in files)
            {
                File.Delete(f);
            }
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {

        }

        public void OnResumePublic()
        {
            OnResume();
        }
    }
}