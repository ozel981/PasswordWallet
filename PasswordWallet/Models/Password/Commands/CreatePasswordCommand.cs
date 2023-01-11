using FluentValidation;

namespace PasswordWallet.Models.Password.Commands
{
    public class CreatePasswordCommand
    {
        public string PasswordText { get; set; } = string.Empty;
        public bool IsPasswordKeptAsHash { get; set; } = false;
        public string WebAdderss { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
    }

    public class CreatePasswordCommandValidator : AbstractValidator<CreatePasswordCommand>
    {
        public const int passwordMaxLength = 512;
        public const int inputMaxLength = 256;
        public CreatePasswordCommandValidator()
        {
            RuleFor(x => x.PasswordText)
                .NotEmpty()
                .MaximumLength(passwordMaxLength);

            RuleFor(x => x.WebAdderss)
                .NotEmpty()
                .MaximumLength(inputMaxLength);

            RuleFor(x => x.Login)
                .NotEmpty()
                .MaximumLength(inputMaxLength);

            RuleFor(x => x.Description)
                .MaximumLength(inputMaxLength);
        }
    }
}
