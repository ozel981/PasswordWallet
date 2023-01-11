namespace PasswordWallet.Models.Register.Commands
{
    public class RaportSigninActionCommand
    {
        public int? UserId { get; set; } = null;

        public string AddressIP { get; set; } = string.Empty;

        public bool IsCorrect { get; set; } = false;

        public string Session { get; set; } = string.Empty;

        public string Computer { get; set; } = string.Empty;
    }
}
