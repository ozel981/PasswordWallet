using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasswordWallet.Authentication;
using PasswordWallet.Database;
using PasswordWallet.Database.Entities;
using PasswordWallet.Models.SharedPassword.Commands;
using PasswordWallet.Models.SharedPassword.Queries;
using PasswordWallet.Models.User.Contracts;
using PasswordWallet.Services.Interfaces;

namespace PasswordWallet.Services.SharedPassword
{
    public class SharedPasswordService : ISharedPasswordService
    {
        private readonly PasswordWalletDbContext _dbContext;
        private readonly IHashService _hashService;

        public SharedPasswordService(PasswordWalletDbContext dbContext, IHashService hashService)
        {
            _dbContext = dbContext;
            _hashService = hashService;
        }

        public async Task<IQueryable<Models.Password.Contracts.Password>> GetSharedPasswords(AuthData authData, GetSharedPasswordsQuery query)
        {
            var user = await _dbContext.Users.FindAsync(authData.UserId);
            var sharedPasswords = _dbContext.SharedPasswords
                .Include(sp => sp.User)
                .Include(sp => sp.Password)
                .Where(sp => sp.User.Id == authData.UserId).ToList();

            if (user == null)
            {
                throw new Exception($"User does not exist");
            }
            else if (!sharedPasswords.IsNullOrEmpty())
            {
                var passwords = sharedPasswords.Select(password => GetSharedPasswor(authData, user, password, false)).ToList();
                await _dbContext.SaveChangesAsync();
                return passwords.AsQueryable();
            }
            else
            {
                return new List<Models.Password.Contracts.Password>().AsQueryable();
            }
        }

        public async Task<int> SharePassword(AuthData authData, SharePasswordCommand command)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login.Equals(command.UserLogin));
            var password = await _dbContext.Passwords.FindAsync(command.PasswordId);

            if (user == null)
            {
                throw new Exception($"User does not exist");
            }
            else if (password == null)
            {
                throw new Exception($"Password does not exist");
            }
            else
            {
                var decryptedPassword = _hashService.Decrypt(password.PasswordText, authData.HashKey);
                var encryptedPassword = _hashService.EncryptByRSA(decryptedPassword, user.PublicKeyJsonStr);

                var sharedPassword = new Database.Entities.SharedPassword()
                {
                    User = user,
                    Password = password,
                    IsNew = true,
                    PasswordText = encryptedPassword,
                };

                await _dbContext.SharedPasswords.AddAsync(sharedPassword);
                await _dbContext.SaveChangesAsync();

                return sharedPassword.Id;
            }
        }

        public async Task<Models.Password.Contracts.Password> GetDecodedSharedPassword(AuthData authData, GetDecodedSharedPasswordQuery query)
        {
            var user = await _dbContext.Users.FindAsync(authData.UserId);
            var sharedPassword = await _dbContext.SharedPasswords.FindAsync(query.Id);

            if (user == null)
            {
                throw new Exception($"User does not exist"); 
            }
            else if (sharedPassword == null)
            {
                throw new Exception($"Password does not exist");
            }
            else
            {
                return GetSharedPasswor(authData, user, sharedPassword, true);
            }
        }

        private Models.Password.Contracts.Password GetSharedPasswor(
            AuthData authData, 
            Database.Entities.User user, 
            Database.Entities.SharedPassword password,
            bool decrypted)
        {
            var passwordText = password.PasswordText;
            if (password.IsNew)
            {
                var privateRSAKey = _hashService.DecryptRSAKey(user.PrivateKeyJsonStr, authData.HashKey);
                var decryptedPassword = _hashService.DecryptByRSA(password.PasswordText, privateRSAKey);
                password.PasswordText = _hashService.Encrypt(decryptedPassword, authData.HashKey);
                password.IsNew = false;
                if (decrypted) passwordText = decryptedPassword;
                else passwordText = password.PasswordText;
            }
            else
            {
                if (decrypted) passwordText = _hashService.Decrypt(password.PasswordText, authData.HashKey);
            }
            return new Models.Password.Contracts.Password()
            {
                Id = password.Id,
                Description = password.Password.Description,
                Login = password.Password.Login,
                PasswordText = passwordText,
                WebAdderss = password.Password.WebAdderss,
            };
        }

        public async Task DeleteSharedPassword(AuthData authData, DeleteSharedPasswordCommand command)
        {
            var user = await _dbContext.Users.FindAsync(authData.UserId);

            if (user != null)
            {
                var password = await _dbContext.SharedPasswords.FindAsync(command.Id);
                if (password != null && password.User.Id == authData.UserId)
                {
                    _dbContext.SharedPasswords.Remove(password);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"User do not have password with that id");
                }
            }
            else
            {
                throw new Exception($"User does not exist");
            }
        }
    }
}
