using PasswordWallet.Authentication;
using PasswordWallet.Models.Password.Commands;
using PasswordWallet.Models.Password.Queries;

namespace PasswordWallet.Services.Interfaces
{
    public interface IPasswordService
    {
        Task<Models.Password.Contracts.Password> CreatePassword(AuthData authData, CreatePasswordCommand command);
        Task<Models.Password.Contracts.Password> UpdatePassword(AuthData authData, UpdatePasswordCommand command);
        Task DeletePassword(AuthData authData, DeletePasswordCommand command);
        Task<IQueryable<Models.Password.Contracts.Password>> GetUserPasswords(GetPasswordsQuery query);
        Task<Models.Password.Contracts.Password> GetDecodedPassword(AuthData authData, GetDecodedPasswordQuery query);
    }
}
