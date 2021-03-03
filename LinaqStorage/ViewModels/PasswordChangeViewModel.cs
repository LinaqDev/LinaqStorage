﻿using LinaqStorage.Helpers;
using System.Linq;
using System.Windows.Input;

namespace LinaqStorage.ViewModels
{
    public class PasswordChangeViewModel : BaseViewModel
    {

        public PasswordChangeViewModel()
        {
            SavePassCmd = new RelayCommand(SavePassExe);
        }
        private string _oldPasswordEntry = "";
        public string OldPasswordEntry
        {
            get
            {
                return _oldPasswordEntry;
            }
            set
            {
                _oldPasswordEntry = value;
                RaisePropertyChanged("OldPasswordEntry");
            }
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


        private string _confirmPasswordEntry = "";
        public string ConfirmPasswordEntry
        {
            get
            {
                return _confirmPasswordEntry;
            }
            set
            {
                _confirmPasswordEntry = value;
                RaisePropertyChanged("ConfirmPasswordEntry");
            }
        }

        public ICommand SavePassCmd { get; set; }

        public string UserPassword
        {
            get
            {
                return App.AppSettings.GetValueOrDefault(nameof(UserPassword), string.Empty);
            }
            set
            {
                App.AppSettings.AddOrUpdateValue(nameof(UserPassword), value);
                RaisePropertyChanged("UserPassword");
            }
        }

        private async void SavePassExe()
        {
            if (OldPasswordEntry == UserPassword)
            {
                if (verifyPassword())
                {
                    UserPassword = _passwordEntry;
                    await NavigationService.GoBack();
                }
            }
            else
            {
                UserDialogsService.ShowAltert(TranslateService.ProvideValue("Message_OldPasswordDoesntMatch"), TranslateService.ProvideValue("Message_WrongPasswrdShort"));
            }
        }

        private bool verifyPassword()
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(_passwordEntry) || string.IsNullOrWhiteSpace(_confirmPasswordEntry) || string.IsNullOrWhiteSpace(_oldPasswordEntry))
            {
                errorMessage += TranslateService.ProvideValue("Message_FilleAllFields") + "\n";
            }
            if (_passwordEntry != _confirmPasswordEntry)
            {
                errorMessage += TranslateService.ProvideValue("Message_PasswordsDoesntMatch") + "\n";
            }
            if (_passwordEntry.Length < 8)
            {
                errorMessage += TranslateService.ProvideValue("Message_PasswordTooShort") + "\n";
            }
            if (!_passwordEntry.Any(char.IsSymbol))
            {
                errorMessage += TranslateService.ProvideValue("Message_PasswordOneSymbol") + "\n";
            }
            if (!_passwordEntry.Any(char.IsUpper))
            {
                errorMessage += TranslateService.ProvideValue("Message_PasswordOneBigLetter") + "\n";
            }
            if (!_passwordEntry.Any(char.IsNumber))
            {
                errorMessage += TranslateService.ProvideValue("Message_PasswordOneNumber") + "\n";
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                UserDialogsService.ShowAltert(errorMessage, TranslateService.ProvideValue("Message_WrongPasswrdShort"));
                return false;
            }

            return true;
        }


    }
}
