
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace strava.personal.bests.api.Services
{
    public class Crypto : ICrypto
    {
        //private readonly IPropertyStore _propertyStore;

        // public Crypto(IPropertyStore propertyStore)
        // {
        //     _propertyStore = propertyStore;
        // }
        public string Encrypt(string key, string data)
        {
            string encData = null;
            //var key = GetKey(property);
            var keys = GetHashKeys(key);

            try
            {
                encData = EncryptStringToBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return encData;
        }

        public string Decrypt(string key, string data)
        {
            var keys = GetHashKeys(key);
            return DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
        }

        // private string GetKey(IApplicationProperty property)
        // {
        //     return _propertyStore.Get(property);
        // }

        private static byte[][] GetHashKeys(string key)
        {
            var result = new byte[2][];
            var enc = Encoding.UTF8;

            SHA256 sha2 = new SHA256CryptoServiceProvider();

            var rawKey = enc.GetBytes(key);
            var rawIV = enc.GetBytes(key);

            var hashKey = sha2.ComputeHash(rawKey);
            var hashIV = sha2.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }

        private static string EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt =
                            new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            var cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt =
                            new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }

    public interface ICrypto
    {
        string Encrypt(string property, string data);
        string Decrypt(string property, string data);
    }
}
