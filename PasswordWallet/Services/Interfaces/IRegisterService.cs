using PasswordWallet.Database.Entities;
using PasswordWallet.Models.Register.Commands;

namespace PasswordWallet.Services.Interfaces
{
    public interface IRegisterService
    {
        Task<int> CreateSignInRegister(RaportSigninActionCommand command, IpAddress ipAddress);
        Task<double?> SetLockTime(RaportSigninActionCommand command, IpAddress ipAddress);
        Task<IpAddress?> GetIpAddress(string addressIP);
        Task<IpAddress> GetOrCreateIpAddress(string addressIP);
    }
}
