using Microsoft.EntityFrameworkCore;
using PasswordWallet.Database;
using PasswordWallet.Services.Interfaces;
using PasswordWallet.Authentication;
using PasswordWallet.Models.User.Commands;
using Microsoft.Win32;

namespace PasswordWallet.Services.User
{
    public class UserService : IUserService
    {
        private readonly PasswordWalletDbContext _dbContext;
        private readonly IHashService _hashService;

        public UserService(PasswordWalletDbContext dbContext, IHashService hashService)
        {
            _dbContext = dbContext;
            _hashService = hashService;
        }

        public async Task UpdateUser(AuthData authData, UpdateUserCommand command)
        {
            var user = await _dbContext.Users.Include(u => u.Passwords).Include(u => u.SharedPasswords).FirstOrDefaultAsync(u => u.Id == authData.UserId);

            if (user != null)
            {
                if(command.Login != null) user.Login = command.Login;
                if (command.Password != null || (command.IsPasswordKeptAsHash != null && command.IsPasswordKeptAsHash != user.IsPasswordKeptAsHash))
                {
                    if (command.Password != null)
                    {
                        var newHashKey = _hashService.GetHashMD5(command.Password);
                        foreach (var password in user.Passwords)
                        {
                            var passwordText = _hashService.Decrypt(password.PasswordText, authData.HashKey);
                            password.PasswordText = _hashService.Encrypt(passwordText, newHashKey);
                        }
                        foreach (var password in user.SharedPasswords)
                        {
                            if (!password.IsNew)
                            {
                                var passwordText = _hashService.Decrypt(password.PasswordText, authData.HashKey);
                                password.PasswordText = _hashService.Encrypt(passwordText, newHashKey);
                            }
                        }
                        user.PrivateKeyJsonStr = _hashService.EncryptRSAKey(_hashService.DecryptRSAKey(user.PrivateKeyJsonStr, authData.HashKey), newHashKey);
                    }
                    var encryptedPasswor = _hashService.GetNewHash(command.Password ?? user.Password, command.IsPasswordKeptAsHash ?? user.IsPasswordKeptAsHash);
                    user.Password = encryptedPasswor.Hash;
                    user.Salt = encryptedPasswor.Salt;
                    if (command.IsPasswordKeptAsHash != null) user.IsPasswordKeptAsHash = command.IsPasswordKeptAsHash.Value;
                }
                _dbContext.SaveChanges();
                return;
            }
            throw new Exception($"User does not exist");
        }

        public async Task<int> CreateUser(CreateUserCommand command)
        {
            if(_dbContext.Users.Any(u => u.Login == command.Login))
            {
                throw new Exception("Login already exists");
            }

            var encryptedPasswor = _hashService.GetNewHash(command.Password, command.IsPasswordKeptAsHash);

            var privateRSAKey = "";
            var publicRSAKey = "";

            (privateRSAKey, publicRSAKey) = _hashService.GetRSAKeys();
            privateRSAKey = _hashService.EncryptRSAKey(privateRSAKey, _hashService.GetHashMD5(command.Password));

            var user = new Database.Entities.User()
            {
                Login = command.Login,
                Password = encryptedPasswor.Hash,
                Salt = encryptedPasswor.Salt,
                IsPasswordKeptAsHash = command.IsPasswordKeptAsHash,
                PrivateKeyJsonStr = privateRSAKey,
                PublicKeyJsonStr = publicRSAKey
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user.Id;
        }

        public async Task<Models.User.Contracts.User> GetUser(int id)
        {
            var dbUser = await _dbContext.Users
                .Include(u => u.SignInRegisters)
                .ThenInclude(r => r.IpAddress)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (dbUser == null) throw new Exception();

            var user = new Models.User.Contracts.User()
            {
                Id = id,
                Login = dbUser.Login
            };

            if (dbUser.SignInRegisters.Count > 1)
            {
                var register = dbUser.SignInRegisters.OrderByDescending(r => r.Id).Take(2).Last();

                user.LastSigninDateTime = register.Date;
                user.WasSuccessfulSignin = register.IsCorrect;
                user.AddressIP = register.IpAddress.AddressIP;
                user.SessionKey = register.Session;
            } 
            else
            {
                user.LastSigninDateTime = DateTime.Now;
                user.WasSuccessfulSignin = true;
                if (dbUser.SignInRegisters.Count == 1)
                {
                    var register = dbUser.SignInRegisters.Single();
                    user.AddressIP = register.IpAddress.AddressIP;
                    user.SessionKey = register.Session;
                }
            }

            return user;
        }
    }
}
