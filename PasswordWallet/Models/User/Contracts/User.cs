namespace PasswordWallet.Models.User.Contracts
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; } = string.Empty;

        public DateTime LastSigninDateTime { get; set; } = DateTime.Now;

        public bool WasSuccessfulSignin { get; set; } = true;

        public string SessionKey { get; set; } = string.Empty;

        public string AddressIP { get; set; } = string.Empty;
    }
}
