using LinaqStorage.Enums;
using System;

namespace LinaqStorage.Models
{
    public class FileObject
    {
        public string FileGuid { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public CryptMethod FileEncryptionMethod { get; set; }
        public DateTime FileEncryptionDate { get; set; }
        public string FileDecryptionPassword { get; set; }
        public bool IsKeyEncrypted { get; set; } 
        public string PublicRsa { get; set; }
        public string PrivateRSA { get; set; }
        public string OriginalFileSize { get; set; }
        public string EncryptedFileSize { get; set; }
        public string IconPath
        {
            get
            {
                string name = $"{FileExtension.Remove(0, 1)}";
                if (!Constants.TypesIconsList.Contains(name))
                    name = $"_blank";

                return $"{name}.png";
            }
        }
    }
}
