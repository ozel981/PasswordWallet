namespace PasswordWallet.Database.Entities
{
    public class SignInRegister
    {
        public int Id { get; private set; }

        public User? User { get; set; } = null;

        public IpAddress IpAddress { get; set; } = new IpAddress();

        public DateTime Date { get; set; } = DateTime.Now;

        public bool IsCorrect { get; set; } = false;

        public string Session { get; set; } = string.Empty;

        public string Computer { get; set; } = string.Empty;
    }
}
