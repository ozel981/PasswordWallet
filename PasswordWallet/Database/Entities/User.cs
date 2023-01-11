namespace PasswordWallet.Database.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public bool IsPasswordKeptAsHash { get; set; } = false;
        public string PublicKeyJsonStr { get; set; } = string.Empty;
        public string PrivateKeyJsonStr { get; set; } = string.Empty;
        public ICollection<Password> Passwords { get; set; } = new HashSet<Password>();

        public ICollection<SignInRegister> SignInRegisters { get; set; } = new HashSet<SignInRegister>();

        public ICollection<SharedPassword> SharedPasswords { get; set; } = new HashSet<SharedPassword>();
    }
}
