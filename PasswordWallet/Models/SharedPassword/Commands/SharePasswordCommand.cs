namespace PasswordWallet.Models.SharedPassword.Commands
{
    public class SharePasswordCommand
    {
        public int PasswordId { get; set; }
        public string UserLogin { get; set; } = string.Empty;
    }
}
