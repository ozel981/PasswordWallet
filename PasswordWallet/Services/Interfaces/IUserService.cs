using PasswordWallet.Authentication;
using PasswordWallet.Models.User.Commands;

namespace PasswordWallet.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUser(CreateUserCommand command);
        Task UpdateUser(AuthData authData, UpdateUserCommand command);
        Task<Models.User.Contracts.User> GetUser(int id);
    }
}
