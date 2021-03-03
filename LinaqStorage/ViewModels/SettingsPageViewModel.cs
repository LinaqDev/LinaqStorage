using LinaqStorage.Enums;
using LinaqStorage.Helpers;
using LinaqStorage.Resources;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

// czy przeszyfrowac stare pliki ?
namespace LinaqStorage.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Constructors
        public SettingsPageViewModel()
        {
            InitSettings();
            ResetSettingsToDefaultCmd = new RelayCommand(ResetSettingsToDefaultExe);
            OpenChangePasswordPageCmd = new RelayCommand(OpenChangePasswordPageExe);
        }

        private async void OpenChangePasswordPageExe()
        {
            await NavigationService.NavigateAsync("PasswordChangePage", true);
        }
        #endregion

        #region Properties
        // delete old file ?
         

        public IEnumerable<CryptMethod> EncryptionMemthods
        {
            get
            {
                return Enum.GetValues(typeof(CryptMethod)).Cast<CryptMethod>();
            }
        }

        public CryptMethod SelectedEncryptionMethod
        {
            get
            {
                string method = App.AppSettings.GetValueOrDefault(nameof(SelectedEncryptionMethod), string.Empty);
                switch (method)
                {
                    case "AES":
                        return CryptMethod.AES;
                    case "DES":
                        return CryptMethod.DES;
                    case "TripleDES":
                        return CryptMethod.TripleDES;
                    default:
                        return CryptMethod.AES;
                }
            }
            set
            {
                App.AppSettings.AddOrUpdateValue(nameof(SelectedEncryptionMethod), value.ToString());
                RaisePropertyChanged("SelectedEncryptionMethod");
            }
        }

        private ObservableCollection<string> _languagesList = new ObservableCollection<string>();
        public ObservableCollection<string> LanguageList
        {
            get
            {
                return _languagesList;
            }
            set
            {
                _languagesList = value;
                RaisePropertyChanged("LanguageList");
            }
        }

        public string SelectedLanguage
        {
            get
            {
                return App.AppSettings.GetValueOrDefault(nameof(SelectedLanguage), string.Empty);
            }
            set
            {
                if (App.AppSettings.GetValueOrDefault(nameof(SelectedLanguage), string.Empty) != value)
                {
                    App.AppSettings.AddOrUpdateValue(nameof(SelectedLanguage), value);
                    RaisePropertyChanged("SelectedLanguage");
                    onLanguageChange(value);
                }
            }
        }

        public string UserName
        {
            get
            {
                return App.AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            }
            set
            {
                App.AppSettings.AddOrUpdateValue(nameof(UserName), value);
                RaisePropertyChanged("UserName");
            }
        }

        public bool DeleteOriginalFile
        {
            get
            {
                return App.AppSettings.GetValueOrDefault(nameof(DeleteOriginalFile), false);
            }
            set
            {
                App.AppSettings.AddOrUpdateValue(nameof(DeleteOriginalFile), value);
                RaisePropertyChanged("DeleteOriginalFile");
            }
        }

        public bool EncryptKeyWithRSA
        {
            get
            {
                return App.AppSettings.GetValueOrDefault(nameof(EncryptKeyWithRSA), true);
            }
            set
            {
                App.AppSettings.AddOrUpdateValue(nameof(EncryptKeyWithRSA), value);
                RaisePropertyChanged("EncryptKeyWithRSA");
            }
        }
        #endregion

        #region Commands
        public ICommand ResetSettingsToDefaultCmd { get; set; }
        public ICommand OpenChangePasswordPageCmd { get; set; }
        #endregion

        #region Methods
        private void ResetSettingsToDefaultExe()
        {
            SetDefaultSettings(true);
        }

        private void InitSettings()
        { 
            LanguageList.Add("pl");
            LanguageList.Add("en");
            SetDefaultSettings(false);
        }

        private void SetDefaultSettings(bool reset)
        {
            if (reset)
                SelectedEncryptionMethod = CryptMethod.AES;
            else if (string.IsNullOrWhiteSpace(SelectedEncryptionMethod.ToString()))
                SelectedEncryptionMethod = CryptMethod.AES;

            if (reset)
                SelectedLanguage = "en";
            else if (string.IsNullOrWhiteSpace(SelectedLanguage))
                SelectedLanguage = "en";
        }

        private async void onLanguageChange(string obj)
        {
            UserDialogsService.ShowLoading(TranslateService.ProvideValue("Loading"));
            CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo(obj);
            AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
            await NavigationService.UpdagePagesLanguage();
            UserDialogsService.HideLoading();
        }
        #endregion
    }
}
