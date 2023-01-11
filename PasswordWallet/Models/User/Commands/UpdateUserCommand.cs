namespace PasswordWallet.Models.User.Commands
{
    public class UpdateUserCommand
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public bool? IsPasswordKeptAsHash { get; set; }
    }
}
