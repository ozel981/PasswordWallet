namespace PasswordWallet.Database.Entities
{
    public class Password
    {
        public int Id { get; private set; }
        public User User { get; set; } = new User();
        public string PasswordText { get; set; } = string.Empty;
        public string WebAdderss { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public ICollection<SharedPassword> SharedPasswords { get; set; } = new HashSet<SharedPassword>();
    }
}
