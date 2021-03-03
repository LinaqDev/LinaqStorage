using LinaqStorage.Helpers;
using LinaqStorage.Helpers.CryptographyMethods;
using System;
using System.Text;
using Xunit;

namespace XUnitTestLinaqStorage
{
    public class CryptographyTests
    {
        string stringToEncrypt = "TestContent";

        [Fact]
        public void TripleDESTest()
        {
            var filebytecontent = Encoding.ASCII.GetBytes(stringToEncrypt);
            string password = Helpers.CreateNewGuidWithoutDash();
            byte[] EncryptedContent = _3DES.Encrypt(filebytecontent, password);
            byte[] DecryptedContent = _3DES.Decrypt(EncryptedContent, password);

            Assert.Equal(stringToEncrypt, Encoding.ASCII.GetString(DecryptedContent));
            
            Console.WriteLine("test");
        }

        [Fact]
        public void DESTest()
        {
            var filebytecontent = Encoding.ASCII.GetBytes(stringToEncrypt);
            string password = Helpers.CreateNewGuidWithoutDash();
            byte[] EncryptedContent = _DES.Encrypt(filebytecontent, password);
            byte[] DecryptedContent = _DES.Decrypt(EncryptedContent, password);

            Assert.Equal(stringToEncrypt, Encoding.ASCII.GetString(DecryptedContent));
        }

        [Fact]
        public void AESTest()
        {
            var filebytecontent = Encoding.ASCII.GetBytes(stringToEncrypt);
            string password = Helpers.CreateNewGuidWithoutDash();
            byte[] EncryptedContent = _AES.Encrypt(filebytecontent, password);
            byte[] DecryptedContent = _AES.Decrypt(EncryptedContent, password);

            Assert.Equal(stringToEncrypt, Encoding.ASCII.GetString(DecryptedContent));
        }

        [Fact]
        public void RSAByetsTest()
        {
            var filebytecontent = Encoding.ASCII.GetBytes(stringToEncrypt);
            Tuple<string, string> Keys = _RSA.GenerateKey();
            byte[] EncryptedContent = _RSA.Encrypt(filebytecontent, Keys.Item1);
            byte[] DecryptedContent = _RSA.Decrypt(EncryptedContent, Keys.Item2);

            Assert.Equal(stringToEncrypt, Encoding.ASCII.GetString(DecryptedContent));
        }

        [Fact]
        public void RSAStringTest()
        {
            string stringToEncrypt = Helpers.CreateNewGuidWithoutDash();
            Tuple<string, string> Keys = _RSA.GenerateKey();
            string EncryptedContent = _RSA.Encrypt(stringToEncrypt, Keys.Item1);
            string DecryptedContent = _RSA.Decrypt(EncryptedContent, Keys.Item2);
            Assert.Equal(stringToEncrypt, DecryptedContent);
        }
    }
}
