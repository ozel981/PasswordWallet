namespace PasswordWallet.Models.Password.Contracts
{
    public class Password
    {
        public int Id { get; set; }
        public string PasswordText { get; set; } = string.Empty;
        public string WebAdderss { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
    }
}
