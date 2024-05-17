using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Repositories.Repository.Implementation
{
    public class EncryptionHelper
    {
        private const string Key = "VrajSecretKey121212"; // Replace with your secret key

        public static string EncryptId(int id)
        {
            byte[] data = Encoding.UTF8.GetBytes(id.ToString());
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = new byte[16];
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] encryptedData = encryptor.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(encryptedData);
                }
            }
        }

        public static int DecryptId(string encryptedId)
        {
            byte[] encryptedData = Convert.FromBase64String(encryptedId);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = new byte[16];
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    return int.Parse(Encoding.UTF8.GetString(decryptedData));
                }
            }
        }
    }
}
