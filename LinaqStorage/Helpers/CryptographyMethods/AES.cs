using System.IO;
using System.Security.Cryptography;

namespace LinaqStorage.Helpers.CryptographyMethods
{
    public static class _AES
    {
        public static byte[] Encrypt(byte[] input,string password)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[] { 0x43, 0x87, 0x23, 0x72 });
            using (MemoryStream ms = new MemoryStream())
            {
                Aes aes = new AesManaged();
                aes.Key = pdb.GetBytes(aes.KeySize / 8);
                aes.IV = pdb.GetBytes(aes.BlockSize / 8);
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(input, 0, input.Length);
                    cs.Close();
                    return ms.ToArray();
                }
            }
        }

        public static byte[] Decrypt(byte[] input, string password)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[] { 0x43, 0x87, 0x23, 0x72 });
            using (MemoryStream ms = new MemoryStream())
            {
                Aes aes = new AesManaged();
                aes.Key = pdb.GetBytes(aes.KeySize / 8);
                aes.IV = pdb.GetBytes(aes.BlockSize / 8);
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(input, 0, input.Length);
                    cs.Close();
                    return ms.ToArray();
                }
            }
        }
    }
}
