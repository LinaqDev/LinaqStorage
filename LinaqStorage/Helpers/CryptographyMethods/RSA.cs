using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace LinaqStorage.Helpers.CryptographyMethods
{
    public static class _RSA
    {
        public static Tuple<string, string> GenerateKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            string publickKey = rsa.ToXmlStringExt(false);
            string privateKey = rsa.ToXmlStringExt(true);

            return new Tuple<string, string>(publickKey, privateKey);
        }

        public static byte[] Encrypt(byte[] plainbytes, string publicKeyXML)
        {
            if (plainbytes.Length > 86)
                throw new Exception("Data too big");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlStringExt(publicKeyXML);
            return rsa.Encrypt(plainbytes, false);
        }

        public static byte[] Decrypt(byte[] plainbytes, string privateKeyXML)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlStringExt(privateKeyXML);
            return rsa.Decrypt(plainbytes, false);
        }

        public static string Encrypt(string str, string publicKeyXML)
        {
            byte[] bytes = Convert.FromBase64String(str);
            if (bytes.Length > 86)
                throw new Exception("Data too big");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlStringExt(publicKeyXML);
            return Convert.ToBase64String(rsa.Encrypt(bytes, false));
        }

        public static string Decrypt(string str, string privateKeyXML)
        {
            byte[] bytes = Convert.FromBase64String(str);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlStringExt(privateKeyXML);
            return Convert.ToBase64String(rsa.Decrypt(bytes, false));
        } 
       

      

        #region Unused
        private static List<byte[]> EncryptData(byte[] plainbytes, string publicOnlyKeyXML)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicOnlyKeyXML);

            List<byte[]> blocks = new List<byte[]>();
            blocks = SplitToSublists(plainbytes, 86);

            List<byte[]> encryptedData = new List<byte[]>();

            foreach (var item in blocks)
            {
                encryptedData.Add(rsa.Encrypt(item, false));
            }
            return encryptedData;
        }

        private static byte[] DecryptData(List<byte[]> blocks, string privateOnlyKeyXML)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateOnlyKeyXML);


            List<byte> dencryptedData = new List<byte>();

            foreach (var item in blocks)
            {
                dencryptedData.AddRange(rsa.Decrypt(item, false));
            }
            return dencryptedData.ToArray<byte>();


            //  return  rsa.Decrypt(plainbytes, false);  
        }

        private static List<byte[]> SplitToSublists(byte[] source, int size)
        {
            return source
                     .Select((x, i) => new { Index = i, Value = x })
                     .GroupBy(x => x.Index / size)
                     .Select(x => x.Select(v => v.Value).ToArray())
                     .ToList();
        }
        #endregion
    }
}
