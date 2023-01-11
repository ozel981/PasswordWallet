
using Newtonsoft.Json;
using PasswordWallet.Services.Hash;
using PasswordWallet.Services.Interfaces;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace PasswordWalletTests.Services
{
    public class HashServiceTests
    {
        private IHashService _hashService;

        public HashServiceTests()
        {
            _hashService = new HashService();
        }

        [Theory]
        [Description("Test hashing password by SHA512")]
        [InlineData("mojehaslo")]
        [InlineData("")]
        [InlineData("?!@*%&#^$")]
        [InlineData("0123456789")]
        public void SHA512Tests(string password)
        {
            var hashData = _hashService.GetNewHash(password, true);

            var hash = _hashService.GetHash(password, hashData.Salt, true);

            Assert.Equal(hashData.Hash, hash);
        }

        [Theory]
        [Description("Test hashing password by HMACSHA512")]
        [InlineData("mojehaslo")]
        [InlineData("")]
        [InlineData("?!@*%&#^$")]
        [InlineData("0123456789")]
        public void HMACSHA512Tests(string password)
        {
            var hashData = _hashService.GetNewHash(password, false);

            var hash = _hashService.GetHash(password, hashData.Salt, false);

            Assert.Equal(hashData.Hash, hash);
        }

        [Theory]
        [Description("Test encoding and decoding texts by password text")]
        [InlineData("mojehaslo","text")]
        [InlineData("","")]
        [InlineData("?!@*%&#^$", "?!@*%&#^$")]
        [InlineData("0123456789", "0123456789")]
        [InlineData("mojehaslo", "")]
        [InlineData("", "text")]
        public void EncodedTests(string password, string text)
        {
            var hashPassword = _hashService.GetHashMD5(password);

            var encrypted = _hashService.Encrypt(text, hashPassword);
            var decrypted = _hashService.Decrypt(encrypted, hashPassword);

            Assert.Equal(text, decrypted);
        }

        [Theory]
        [Description("Test MD5 deterministic")]
        [InlineData("mojehaslo")]
        [InlineData("")]
        [InlineData("?!@*%&#^$")]
        [InlineData("0123456789")]
        public void MD5Tests(string text)
        {
            var has1 = _hashService.GetHashMD5(text);
            var has2 = _hashService.GetHashMD5(text);

            Assert.Equal(has1, has2);
        }

        [Fact]
        public void RseEncryptDecrypt()
        {
            string privateKey = string.Empty;
            string publicKey = string.Empty;
            (privateKey, publicKey) = _hashService.GetRSAKeys();
            var text = "mypasswordtoshare";
            var encrypted = _hashService.EncryptByRSA(text, publicKey);
            var decrypted = _hashService.DecryptByRSA(encrypted, privateKey);
            Assert.Equal(text, decrypted);
        }

        [Fact]
        public void EncryptDecryptRSAKey()
        {
            string key = _hashService.GetHashMD5("wojtek");
            string privateKey = string.Empty;
            string publicKey = string.Empty;
            (privateKey, publicKey) = _hashService.GetRSAKeys();
            var encryptedKey = "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";
            var decryptedKey = _hashService.DecryptRSAKey(encryptedKey, key);

            Assert.Equal(decryptedKey, privateKey);
        }
    }
}
