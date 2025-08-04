using System.Security.Cryptography;
using System.Text;

namespace HumanCapitalManagementApp.Utilities
{
    public static class EncryptionHelper
    {
    
            private static readonly string key = "A1B2C3D4E5F6G7H8I9J0K1L2M3N4O5P6"; // 32 chars
            private static readonly string iv = "1A2B3C4D5E6F7G8H"; // 16 chars

            public static string Encrypt(string plainText)
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                    sw.Write(plainText);

                return Convert.ToBase64String(ms.ToArray());
            }

            public static string Decrypt(string cipherText)
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
        
    }
}
