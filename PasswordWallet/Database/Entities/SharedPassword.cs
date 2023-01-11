namespace PasswordWallet.Database.Entities
{
    public class SharedPassword
    {
        public int Id { get; private set; }
        public User User { get; set; } = new User();
        public Password Password { get; set; } = new Password();
        public string PasswordText { get; set; } = string.Empty;
        public bool IsNew { get; set; } = true;
    }
}
