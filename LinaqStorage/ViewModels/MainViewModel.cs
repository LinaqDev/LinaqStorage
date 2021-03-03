using Acr.UserDialogs;
using LinaqStorage.Enums;
using LinaqStorage.Helpers;
using LinaqStorage.Helpers.CryptographyMethods;
using LinaqStorage.Interfaces;
using LinaqStorage.Models;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LinaqStorage.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Constructors
        public MainViewModel()
        {
            SettingsPageCmd = new RelayCommand(SettingsPageExe);
            AddNewFileCmd = new RelayCommand(AddNewFileExe);
            OpenFileCmd = new RelayCommand(OpenFileExe);
            DeleteFileObjectCmd = new RelayCommand<object>(DeleteFileObjectExe);
            AboutPageCmd = new RelayCommand(AboutPageExe);
            DetailsPageCmd = new RelayCommand<object>(DetailsPageExe);
            HelpPageCmd = new RelayCommand(HelpPageExe);
            SaveAsCmd = new RelayCommand<object>(SaveAsExe);
            ExitAppCmd = new RelayCommand(ExitAppExe);
            string test = TranslateService.ProvideValue("NavigationBar_Settings");
            InitSortList();
        }

        #endregion

        #region Commands
        public ICommand SettingsPageCmd { get; set; }
        public ICommand OpenFileCmd { get; set; }
        public ICommand AddNewFileCmd { get; set; }
        public ICommand DeleteFileObjectCmd { get; set; }
        public ICommand AboutPageCmd { get; set; }
        public ICommand DetailsPageCmd { get; set; }
        public ICommand HelpPageCmd { get; set; }
        public ICommand ExitAppCmd { get; set; }
        public ICommand SaveAsCmd { get; set; }
        #endregion

        #region Properties 
        private FileObject _selectedFileObject = new FileObject();
        public FileObject SelectedFileObject
        {
            get
            {
                return _selectedFileObject;
            }
            set
            {
                _selectedFileObject = value;
                RaisePropertyChanged("SelectedFileObject");
            }
        }

        private ObservableCollection<FileObject> _filesList = new ObservableCollection<FileObject>();
        public ObservableCollection<FileObject> FilesList
        {
            get
            {
                return _filesList;
            }
            set
            {
                _filesList = value;
                RaisePropertyChanged("FilesList");
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

        public bool EncryptKeyWithRSA
        {
            get
            {
                return App.AppSettings.GetValueOrDefault(nameof(EncryptKeyWithRSA), true);
            }
        }

        public string SortListProperty
        {
            get
            {
                return App.AppSettings.GetValueOrDefault(nameof(SortListProperty), "FileName");
            }
            set
            {
                App.AppSettings.AddOrUpdateValue(nameof(SortListProperty), value);
                RaisePropertyChanged("SortListProperty");
            }
        }

        private CustomComboboxItem _selectedSortProperty;
        public CustomComboboxItem SelectedSortProperty
        {
            get { return _selectedSortProperty; }
            set
            {
                _selectedSortProperty = value;
                SortListProperty = value.Value;
                UpdateFilesList();
                RaisePropertyChanged("SelectedSortProperty");
            }
        }

        private ObservableCollection<CustomComboboxItem> _sortListProperties = new ObservableCollection<CustomComboboxItem>();
        public ObservableCollection<CustomComboboxItem> SortListProperties
        {
            get { return _sortListProperties; }
            set
            {
                _sortListProperties = value;
                RaisePropertyChanged("SortListProperties");
            }
        }

        #endregion

        #region Methods
        private void InitSortList()
        {
            SortListProperties.Add(new CustomComboboxItem()
            {
                DisplayText = TranslateService.ProvideValue("FileName"),
                Value = "FileName"
            });

            SortListProperties.Add(new CustomComboboxItem()
            {
                DisplayText = TranslateService.ProvideValue("FileExt"),
                Value = "FileExtension"
            });

            SortListProperties.Add(new CustomComboboxItem()
            {
                DisplayText = TranslateService.ProvideValue("EncryptDate"),
                Value = "FileEncryptionDate"
            });

            SortListProperties.Add(new CustomComboboxItem()
            {
                DisplayText = TranslateService.ProvideValue("EncryptMethod"),
                Value = "FileEncryptionMethod"
            });

            SelectedSortProperty = SortListProperties.FirstOrDefault(x => x.Value == SortListProperty);
        }

        private void OpenFileExe()
        {
            try
            {
                UserDialogsService.ShowLoading("Decrypting file...");
                var bytes = default(byte[]);
                var DecryptedData = default(byte[]);
                using (var streamReader = new StreamReader(SelectedFileObject.FilePath))
                {

                    using (var memstream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memstream);
                        bytes = memstream.ToArray();
                    }
                }

                switch (SelectedFileObject.FileEncryptionMethod)
                {
                    case CryptMethod.AES:
                        DecryptedData = _AES.Decrypt(bytes, SelectedFileObject.FileDecryptionPassword);
                        break;
                    case CryptMethod.DES:
                        DecryptedData = _DES.Decrypt(bytes, SelectedFileObject.FileDecryptionPassword);
                        break;
                    case CryptMethod.TripleDES:
                        DecryptedData = _3DES.Decrypt(bytes, SelectedFileObject.FileDecryptionPassword);
                        break;
                    default:
                        break;
                }

                string decryptedFilePath = SaveFileContentToFile(DecryptedData, $"tmpDecrypted_{Path.GetFileName(SelectedFileObject.FilePath)}");

                UserDialogsService.HideLoading();

                DependencyService.Get<IDocumentOpener>().OpenFile(decryptedFilePath);
            }
            catch (Exception ex)
            {
                UserDialogsService.HideLoading();
                //UserDialogsService.DisplayException(ex);
                UserDialogsService.ShowAltert(TranslateService.ProvideValue("CantOpenFile"), "");
            }
        }

        private void DeleteFileObjectExe(object obj)
        {
            if (obj != null && obj is FileObject)
            {
                JsonService.RemoveFileFromList(obj as FileObject);
                UpdateFilesList();
                RaisePropertyChanged("FilesList");
            }
        }

        private string SaveFileContentToFile(byte[] content, string filename)
        {
            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename);
                using (Stream file = File.OpenWrite(filePath))
                {
                    file.Write(content, 0, content.Length);
                }
                return filePath;
            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
                return string.Empty;
            }
        }

        private void UpdateFilesList()
        {
            var tmp = new ObservableCollection<FileObject>(JsonService.GetFilesListFromJson());
            switch (SelectedSortProperty.Value)
            {
                case "FileName":
                    FilesList = new ObservableCollection<FileObject>(tmp.OrderBy(x => x.FileName).ToList());
                    break;
                case "FileExtension":
                    FilesList = new ObservableCollection<FileObject>(tmp.OrderBy(x => x.FileExtension).ToList());
                    break;
                case "FileEncryptionDate":
                    FilesList = new ObservableCollection<FileObject>(tmp.OrderBy(x => x.FileEncryptionDate).ToList());
                    break;
                case "FileEncryptionMethod":
                    FilesList = new ObservableCollection<FileObject>(tmp.OrderBy(x => x.FileEncryptionMethod).ToList());
                    break;
                default:
                    FilesList = new ObservableCollection<FileObject>(tmp.OrderBy(x => x.FileName).ToList());
                    break;
            }

        }

        private async void SettingsPageExe()
        {
            await NavigationService.NavigateAsync("SettingsPage", "");
        }

        private async void AboutPageExe()
        {
            await NavigationService.NavigateAsync("AboutPage", true);
        }

        private async void AddNewFileExe()
        {
            if (await PermissionsService.GetStoragePermissionsAsync())
            {
                SelectFile();
            }
        }

        private async void SelectFile()
        {
            try
            {

                UserDialogsService.ShowLoading("Encrypting...");
                FileData fileData = new FileData();
                await Task.Run(async () =>
                {

                    fileData = await CrossFilePicker.Current.PickFile();
                    if (fileData == null)
                    {
                        UserDialogsService.HideLoading();
                        return; // user canceled file picking 
                    }

                    string fileName = fileData.FileName;
                    CreateNewFileObject(fileData.DataArray, fileData.FileName);
                    UpdateFilesList();

                    if (App.AppSettings.GetValueOrDefault("DeleteOriginalFile", false))
                        if (File.Exists(fileData.FilePath))
                        {
                            File.Delete(fileData.FilePath);
                            UserDialogs.Instance.Toast("Oryginalny plik został usuniety."); // zmienic na male okienko, dodac zmienna do settingsow czy usuwac czy nie 
                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Usuwanie oryginalnego pliku nie powiodlo sie.");
                        }
                    UserDialogsService.HideLoading();
                });
            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
            }
        }

        private void CreateNewFileObject(byte[] DataArray, string fdFileName)
        {
            try
            {
                string NewgGuid = Helpers.Helpers.CreateNewGuid;
                byte[] EncryptedData = default(byte[]);
                string psw = Helpers.Helpers.CreateNewGuidWithoutDash();
                FileObject NewObject = new FileObject()
                {
                    FileGuid = NewgGuid,
                    FileName = fdFileName,
                    FileEncryptionDate = DateTime.Now,
                    FileExtension = Path.GetExtension(fdFileName),
                    IsKeyEncrypted = false,
                    OriginalFileSize = DataArray.Length.ToString()
                };

                if (EncryptKeyWithRSA)
                {
                    Tuple<string, string> Keys = _RSA.GenerateKey();
                    psw = _RSA.Encrypt(psw, Keys.Item1);
                    NewObject.IsKeyEncrypted = true;
                    NewObject.PublicRsa = Keys.Item1;
                    NewObject.PrivateRSA = Keys.Item2;
                }

                switch (SelectedEncryptionMethod)
                {
                    case CryptMethod.AES:
                        NewObject.FileDecryptionPassword = psw;
                        NewObject.FileEncryptionMethod = SelectedEncryptionMethod;
                        EncryptedData = _AES.Encrypt(DataArray, NewObject.FileDecryptionPassword);
                        NewObject.FilePath = SaveFileContentToFile(EncryptedData, $"{NewgGuid}{Path.GetExtension(fdFileName)}");
                        NewObject.EncryptedFileSize = EncryptedData.Length.ToString();
                        break;
                    case CryptMethod.DES:
                        NewObject.FileDecryptionPassword = psw;
                        NewObject.FileEncryptionMethod = SelectedEncryptionMethod;
                        EncryptedData = _DES.Encrypt(DataArray, NewObject.FileDecryptionPassword);
                        NewObject.FilePath = SaveFileContentToFile(EncryptedData, $"{NewgGuid}{Path.GetExtension(fdFileName)}");
                        NewObject.EncryptedFileSize = EncryptedData.Length.ToString();
                        break;
                    case CryptMethod.TripleDES:
                        NewObject.FileDecryptionPassword = psw;
                        NewObject.FileEncryptionMethod = SelectedEncryptionMethod;
                        EncryptedData = _3DES.Encrypt(DataArray, NewObject.FileDecryptionPassword);
                        NewObject.FilePath = SaveFileContentToFile(EncryptedData, $"{NewgGuid}{Path.GetExtension(fdFileName)}");
                        NewObject.EncryptedFileSize = EncryptedData.Length.ToString();
                        break;
                    default:
                        NewObject.FileDecryptionPassword = psw;
                        NewObject.FileEncryptionMethod = SelectedEncryptionMethod;
                        EncryptedData = _AES.Encrypt(DataArray, NewObject.FileDecryptionPassword);
                        NewObject.FilePath = SaveFileContentToFile(EncryptedData, $"{NewgGuid}{Path.GetExtension(fdFileName)}");
                        NewObject.EncryptedFileSize = EncryptedData.Length.ToString();
                        break;
                }

                JsonService.AddFileToJsonList(NewObject);
            }
            catch (Exception ex)
            {
                UserDialogsService.DisplayException(ex);
            }
        }

        private async void DetailsPageExe(object obj)
        {
            if (obj != null)
            {
                await NavigationService.NavigateAsync("DetailsPage", obj);
            }
        }

        private async void HelpPageExe()
        {
            await NavigationService.NavigateAsync("HelpPage", true);
        }

        private void ExitAppExe()
        {
            DependencyService.Get<ICloseApplication>().closeApplication();
        }
        private async void SaveAsExe(object obj)
        {
            try
            {
                UserDialogsService.ShowLoading("Zapisywanie...");
                FileObject fo;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                await Task.Run(async () =>
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
                {
                    if (obj is FileObject)
                        fo = obj as FileObject;
                    else
                        return;

                    var bytes = default(byte[]);
                    var DecryptedData = default(byte[]);
                    using (var streamReader = new StreamReader(fo.FilePath))
                    {

                        using (var memstream = new MemoryStream())
                        {
                            streamReader.BaseStream.CopyTo(memstream);
                            bytes = memstream.ToArray();
                        }
                    }

                    switch (fo.FileEncryptionMethod)
                    {
                        case CryptMethod.AES:
                            DecryptedData = _AES.Decrypt(bytes, fo.FileDecryptionPassword);
                            break;
                        case CryptMethod.DES:
                            DecryptedData = _DES.Decrypt(bytes, fo.FileDecryptionPassword);
                            break;
                        case CryptMethod.TripleDES:
                            DecryptedData = _3DES.Decrypt(bytes, fo.FileDecryptionPassword);
                            break;
                        default:
                            break;
                    }
                    DependencyService.Get<ISaveFile>().SaveFile(DecryptedData, fo.FileName);
                    UserDialogsService.HideLoading();
                }); 
            }
            catch (Exception)
            {
                UserDialogsService.HideLoading();
                UserDialogs.Instance.Alert("Zapisywanie zakończone nipowodzeniem.", "Błąd");
            }
        }
        #endregion  
    }
}