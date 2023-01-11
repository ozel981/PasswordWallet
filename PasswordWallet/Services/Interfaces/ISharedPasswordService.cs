using PasswordWallet.Authentication;
using PasswordWallet.Models.Password.Queries;
using PasswordWallet.Models.Register.Commands;
using PasswordWallet.Models.SharedPassword.Commands;
using PasswordWallet.Models.SharedPassword.Queries;

namespace PasswordWallet.Services.Interfaces
{
    public interface ISharedPasswordService
    {
        Task<int> SharePassword(AuthData authData, SharePasswordCommand command);
        Task DeleteSharedPassword(AuthData authData, DeleteSharedPasswordCommand command);
        Task<IQueryable<Models.Password.Contracts.Password>> GetSharedPasswords(AuthData authData, GetSharedPasswordsQuery query);
        Task<Models.Password.Contracts.Password> GetDecodedSharedPassword(AuthData authData, GetDecodedSharedPasswordQuery query);
    }
}
