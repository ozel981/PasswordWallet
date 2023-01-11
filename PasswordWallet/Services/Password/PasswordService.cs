using Microsoft.EntityFrameworkCore;
using PasswordWallet.Database;
using PasswordWallet.Services.Interfaces;
using PasswordWallet.Authentication;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using PasswordWallet.Models.Password.Commands;
using PasswordWallet.Models.Password.Queries;

namespace PasswordWallet.Services.Password
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordWalletDbContext _dbContext;
        private readonly IHashService _hashService;

        public PasswordService(PasswordWalletDbContext dbContext, IHashService hashService)
        {
            _dbContext = dbContext;
            _hashService = hashService;
        }

        public async Task<Models.Password.Contracts.Password> CreatePassword(AuthData authData, CreatePasswordCommand command)
        {
            var user = await _dbContext.Users.FindAsync(authData.UserId);

            if (user != null)
            {
                var encryptedPassword = _hashService.Encrypt(command.PasswordText, authData.HashKey);

                var password = new Database.Entities.Password()
                {
                    Login = command.Login,
                    PasswordText = encryptedPassword,
                    WebAdderss = command.WebAdderss,
                    Description = command.Description,
                    User = user,
                };

                await _dbContext.Passwords.AddAsync(password);
                await _dbContext.SaveChangesAsync();

                return new PasswordWallet.Models.Password.Contracts.Password()
                {
                    Id = password.Id,
                    Description = password.Description,
                    Login = password.Login,
                    PasswordText = password.PasswordText,
                    WebAdderss = password.WebAdderss,
                };
            }
            else
            {
                throw new Exception($"User does not exist");
            }
        }

        public async Task DeletePassword(AuthData authData, DeletePasswordCommand command)
        {
            var user = await _dbContext.Users.FindAsync(authData.UserId);

            if (user != null)
            {
                var password = await _dbContext.Passwords.Include(p => p.SharedPasswords).FirstOrDefaultAsync(p => p.Id == command.Id);
                if (password != null && password.User.Id == authData.UserId)
                {
                    _dbContext.SharedPasswords.RemoveRange(password.SharedPasswords);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.Passwords.Remove(password);
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

        public async Task<IQueryable<Models.Password.Contracts.Password>> GetUserPasswords(GetPasswordsQuery query)
        {
            var user = await _dbContext.Users.Include(u => u.Passwords).FirstOrDefaultAsync(u => u.Id == query.UserId);

            if (user != null)
            {
                return user.Passwords.Select(password => new PasswordWallet.Models.Password.Contracts.Password()
                {
                    Id = password.Id,
                    Description = password.Description,
                    Login = password.Login,
                    PasswordText = password.PasswordText,
                    WebAdderss = password.WebAdderss,
                }).AsQueryable();
            }
            else
            {
                throw new Exception($"User does not exist");
            }
        }

        public async Task<Models.Password.Contracts.Password> GetDecodedPassword(AuthData authData, GetDecodedPasswordQuery query)
        {
            var password = await _dbContext.Passwords.FindAsync(query.Id);

            if (password == null) throw new Exception("No such password");

            if (password.User.Id != authData.UserId) throw new Exception("User do not have password with that id");

            var decodedPassword = _hashService.Decrypt(password.PasswordText, authData.HashKey);

            return new PasswordWallet.Models.Password.Contracts.Password()
            {
                Id = password.Id,
                Description = password.Description,
                Login = password.Login,
                PasswordText = decodedPassword,
                WebAdderss = password.WebAdderss,
            };
        }

        public async Task<Models.Password.Contracts.Password> UpdatePassword(AuthData authData, UpdatePasswordCommand command)
        {
            var user = await _dbContext.Users.FindAsync(authData.UserId);

            if (user != null)
            {
                var password = await _dbContext.Passwords.FindAsync(command.Id);
                if (password != null && password.User.Id == authData.UserId)
                {
                    if (command.Login != null) password.Login = command.Login;
                    if (command.WebAdderss != null) password.WebAdderss = command.WebAdderss;
                    if (command.Description != null) password.Description = command.Description;
                    if (command.PasswordText != null) password.PasswordText = _hashService.Encrypt(command.PasswordText, authData.HashKey);

                    await _dbContext.SaveChangesAsync();

                    return new PasswordWallet.Models.Password.Contracts.Password()
                    {
                        Id = password.Id,
                        Description = password.Description,
                        Login = password.Login,
                        PasswordText = password.PasswordText,
                        WebAdderss = password.WebAdderss,
                    };
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
