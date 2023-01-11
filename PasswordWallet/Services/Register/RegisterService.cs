using Microsoft.EntityFrameworkCore.Internal;
using PasswordWallet.Database;
using PasswordWallet.Database.Entities;
using PasswordWallet.Models.Register.Commands;
using PasswordWallet.Services.Interfaces;

namespace PasswordWallet.Services.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly PasswordWalletDbContext _dbContext;

        public RegisterService(PasswordWalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> CreateSignInRegister(RaportSigninActionCommand command, IpAddress ipAddress)
        {
            var now = DateTime.Now;
            var signInRegister = new SignInRegister();
            signInRegister.IsCorrect = command.IsCorrect;
            signInRegister.Session = command.Session;
            signInRegister.Computer = command.Computer;
            signInRegister.Date = now;
            signInRegister.IpAddress = ipAddress;
            if (command.UserId.HasValue)
            {
                var user = await _dbContext.Users.FindAsync(command.UserId.Value);
                signInRegister.User = user;
            }
            if (command.IsCorrect)
            {
                ipAddress.LastSuccessLoginDate = now;
                ipAddress.LockDate = null;
                ipAddress.IncorrectTrialsNumber = 0;
            }
            await _dbContext.SignInRegisters.AddAsync(signInRegister);
            await _dbContext.SaveChangesAsync();
            return signInRegister.Id;
        }

        public async Task<double?> SetLockTime(RaportSigninActionCommand command, IpAddress ipAddress)
        {
            double? lockTime = null;
            var now = DateTime.Now;
            ipAddress.LastBadLoginDate = now;
            ipAddress.IncorrectTrialsNumber++;
            switch (ipAddress.IncorrectTrialsNumber)
            {
                case 1: break;
                case 2: break;
                case 3:
                    {
                        lockTime = 2;
                        ipAddress.LockDate = now.AddSeconds(lockTime.Value);
                    } break;
                case 4:
                    {
                        lockTime = 5;
                        ipAddress.LockDate = now.AddSeconds(lockTime.Value);
                    } break;
                default: 
                    {
                        lockTime = (ipAddress.IncorrectTrialsNumber - 4) * 10;
                        ipAddress.LockDate = now.AddSeconds(lockTime.Value); 
                    } break;
            }

            await _dbContext.SaveChangesAsync();

            return lockTime;
        }

        public async Task<IpAddress> GetOrCreateIpAddress(string addressIP)
        {
            var dbIpAddress = await GetIpAddress(addressIP);
            if (dbIpAddress == null)
            {
                var ipAddress = new IpAddress()
                {
                    AddressIP = addressIP,
                };
                await _dbContext.IpAddresses.AddAsync(ipAddress);
                await _dbContext.SaveChangesAsync();
                return ipAddress;
            }
            else
            {
                return dbIpAddress;
            }
        }

        public async Task<IpAddress?> GetIpAddress(string addressIP)
        {
            return await _dbContext.IpAddresses.FindAsync(addressIP);
        }
    }
}
