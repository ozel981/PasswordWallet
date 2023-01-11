using PasswordWallet.Models.User.Commands;

namespace PasswordWallet.Services.Interfaces
{
    public interface IHashService
    {
        public enum HashType
        {
            MD5,
            SHA512,
            HMACSHA512
        }

        string Encrypt(string text, string key);
        string EncryptRSAKey(string text, string key);
        string Decrypt(string text, string key);
        string DecryptRSAKey(string text, string key);
        (string Hash, string Salt) GetNewHash(string text, bool asHash);
        string GetHash(string text, string salt, bool asHash);
        public string GetHashMD5(string text);
        public string DecryptByRSA(string text, string rsaPrivateKey);
        public string EncryptByRSA(string text, string rsaPublicKey);
        public (string PrivateKey, string PublicKey) GetRSAKeys();
    }
}
