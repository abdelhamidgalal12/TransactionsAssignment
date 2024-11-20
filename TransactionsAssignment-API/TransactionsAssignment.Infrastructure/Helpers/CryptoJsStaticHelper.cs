using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TransactionsAssignment.Infrastructure.Helpers
{
    public static class CryptoJsStaticHelper
    {
        public static string ProcessDecryption(string encryptedTransactionData, string encryptionKey)
        {
            string[] parts = encryptedTransactionData.Split(':');
            byte[] iv = Convert.FromBase64String(parts[0]);
            byte[] ciphertext = Convert.FromBase64String(parts[1]);
            byte[] key = Convert.FromBase64String(encryptionKey);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var memoryStream = new System.IO.MemoryStream(ciphertext))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var reader = new System.IO.StreamReader(cryptoStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}