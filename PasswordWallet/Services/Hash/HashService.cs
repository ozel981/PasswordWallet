using Newtonsoft.Json;
using PasswordWallet.Services.Interfaces;
using System.Buffers.Text;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using static PasswordWallet.Services.Interfaces.IHashService;

namespace PasswordWallet.Services.Hash
{
    public class HashService : IHashService
    {
        private const string pepper = "vWW2Ahm8BR5mJ0yUVVVzCyks60rz5";
        protected const string key = "cG18GFzpczANZyZHbtnP";
        private readonly byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private const int BlockSize = 128;
        private readonly UnicodeEncoding byteConverter = new UnicodeEncoding();

        private const int AesBlockByteSize = BlockSize / 8;

        private const int saltLength = 12;
        public byte[] GetSalt { get => RandomNumberGenerator.GetBytes(saltLength); }
        public string GetStringSalt { get => Convert.ToBase64String(RandomNumberGenerator.GetBytes(saltLength)); }
        public (string Hash, string Salt) GetNewHash(string text, bool asHash)
        {
            return GetNewHash(text, asHash ? HashType.SHA512 : HashType.HMACSHA512);
        }

        public string GetHash(string text, string salt, bool asHash)
        {
            return GetHash(text, salt, asHash ? HashType.SHA512 : HashType.HMACSHA512);
        }
        public (string Hash, string Salt) GetNewHash(string text, HashType hashType)
        {
            string salt = GetStringSalt;
            string hash = GetHash(text, salt, hashType);
            return (hash, salt);
        }

        public string GetHash(string text, string salt, HashType hashType)
        {
            string hash;
            string newText = $"{salt}{pepper}{text}";
            switch (hashType)
            {
                case HashType.HMACSHA512: hash = GetHashHMACSHA512(newText); break;
                case HashType.SHA512: hash = GetHashSHA512(newText); break;
                case HashType.MD5: hash = GetHashMD5(newText); break;
                default: hash = newText; break;
            }
            return hash;
        }

        public string GetHashSHA512(string text)
        {
            var byteData = Encoding.Unicode.GetBytes(text);
            byte[] hash;
            using (SHA512 shaM = SHA512.Create())
            {
                hash = shaM.ComputeHash(byteData);
            }
            return Convert.ToBase64String(hash);
        }

        public string GetHashHMACSHA512(string text, string key = key)
        {
            var byteData = Encoding.Unicode.GetBytes(text);
            var byteKey  = Encoding.Unicode.GetBytes(key);
            byte[] hash;
            using (var hmacsha512 = new HMACSHA512(byteKey))
            {
                hash = hmacsha512.ComputeHash(byteData);
            }
            return Convert.ToBase64String(hash);
        }

        public string GetHashMD5(string text)
        {
            byte[] hashBytes;
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.Unicode.GetBytes(text);
                hashBytes = md5.ComputeHash(inputBytes);
            }
            return Convert.ToBase64String(hashBytes);
        }

        public string Encrypt(string text, string key)
        {
            var result = EncryptAes(text, Convert.FromBase64String(key));
            return Convert.ToBase64String(result);
        }

        public string Decrypt(string text, string key)
        {
            string result = DecryptAes(Convert.FromBase64String(text), Convert.FromBase64String(key));
            return result;
        }

        public string EncryptRSAKey(string text, string key)
        {
            byte[] keyBytes = Convert.FromBase64String(key);
            using (var aes = Aes.Create())
            {
                var iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

                using (var encryptor = aes.CreateEncryptor(keyBytes, iv))
                {
                    var plainText = byteConverter.GetBytes(text);
                    var cipherText = encryptor
                        .TransformFinalBlock(plainText, 0, plainText.Length);

                    var result = new byte[iv.Length + cipherText.Length];
                    iv.CopyTo(result, 0);
                    cipherText.CopyTo(result, iv.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        public string DecryptRSAKey(string text, string key)
        {
            byte[] encryptedData = Convert.FromBase64String(text);
            byte[] keyBytes = Convert.FromBase64String(key);
            using (var aes = Aes.Create())
            {
                var iv = encryptedData.Take(AesBlockByteSize).ToArray();
                var cipherText = encryptedData.Skip(AesBlockByteSize).ToArray();

                using (var encryptor = aes.CreateDecryptor(keyBytes, iv))
                {
                    var decryptedBytes = encryptor
                        .TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return byteConverter.GetString(decryptedBytes);
                }
            }
        }

        public byte[] EncryptAes(string toEncrypt, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                var iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

                using (var encryptor = aes.CreateEncryptor(key, iv))
                {
                    var plainText = Encoding.UTF8.GetBytes(toEncrypt);
                    var cipherText = encryptor
                        .TransformFinalBlock(plainText, 0, plainText.Length);

                    var result = new byte[iv.Length + cipherText.Length];
                    iv.CopyTo(result, 0);
                    cipherText.CopyTo(result, iv.Length);

                    return result;
                }
            }
        }

        public string DecryptAes(byte[] encryptedData, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                var iv = encryptedData.Take(AesBlockByteSize).ToArray();
                var cipherText = encryptedData.Skip(AesBlockByteSize).ToArray();

                using (var encryptor = aes.CreateDecryptor(key, iv))
                {
                    var decryptedBytes = encryptor
                        .TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        public (string PrivateKey, string PublicKey) GetRSAKeys()
        {
            string privateKey = "";
            string publicKey= "";
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                publicKey = JsonConvert.SerializeObject(RSA.ExportParameters(false));
                privateKey = JsonConvert.SerializeObject(RSA.ExportParameters(true));
            }
            return (privateKey, publicKey);
        }

        public string EncryptByRSA(string text, string rsaPublicKey)
        {
            var rsaParameters = JsonConvert.DeserializeObject<RSAParameters>(rsaPublicKey);
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(rsaParameters);
                    encryptedData = RSA.Encrypt(byteConverter.GetBytes(text), false);
                }
                return Convert.ToBase64String(encryptedData);
            }
            catch (CryptographicException e)
            {
                return "";
            }
        }

        public string DecryptByRSA(string text, string rsaPrivateKey)
        {
            var rsaParameters = JsonConvert.DeserializeObject<RSAParameters>(rsaPrivateKey);
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(rsaParameters);
                    decryptedData = RSA.Decrypt(Convert.FromBase64String(text), false);
                }
                return byteConverter.GetString(decryptedData);
            }
            catch (CryptographicException e)
            {
                return "";
            }
        }
    }
}
