using FluentValidation;
using PasswordWallet.Database;
using PasswordWallet.Models.Password.Commands;

namespace PasswordWallet.Models.User.Commands
{
    public class CreateUserCommand
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsPasswordKeptAsHash { get; set; } = false;
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public const int passwordMaxLength = 512;
        public const int loginMaxLength = 30;
        public CreateUserCommandValidator(PasswordWalletDbContext dbContext)
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .MaximumLength(loginMaxLength)
                .Must(login => !dbContext.Users.Any(u => u.Login == login)).WithMessage(x => $"User with {x.Login} already exist");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(passwordMaxLength);
        }
    }
}
