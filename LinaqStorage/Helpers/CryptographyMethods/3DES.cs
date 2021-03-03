using System;
using System.Security.Cryptography;

namespace LinaqStorage.Helpers.CryptographyMethods
{
    public class _3DES
    {
        public static byte[] Encrypt(byte[] input, string password)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[] { 0x43, 0x87, 0x23, 0x72 });
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = pdb.GetBytes(des.KeySize / 8);
            des.IV = new byte[des.BlockSize / 8];

            ICryptoTransform ct = des.CreateEncryptor();
            return ct.TransformFinalBlock(input, 0, input.Length);
        }

        public static byte[] Decrypt(byte[] input, string password)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[] { 0x43, 0x87, 0x23, 0x72 });
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = pdb.GetBytes(des.KeySize / 8);
            des.IV = new byte[des.BlockSize / 8];

            ICryptoTransform ct = des.CreateDecryptor();
            return ct.TransformFinalBlock(input, 0, input.Length);
        }
    }
}