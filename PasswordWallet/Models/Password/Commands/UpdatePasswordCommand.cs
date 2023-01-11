using FluentValidation;

namespace PasswordWallet.Models.Password.Commands
{
    public class UpdatePasswordCommand
    {
        public int Id { get; set; }
        public string? PasswordText { get; set; }
        public bool? IsPasswordKeptAsHash { get; set; }
        public string? WebAdderss { get; set; }
        public string? Description { get; set; }
        public string? Login { get; set; }

        public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
        {
            public const int passwordMaxLength = 512;
            public const int inputMaxLength = 256;
            public UpdatePasswordCommandValidator()
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
}
