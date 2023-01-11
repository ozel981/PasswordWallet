namespace PasswordWallet.Database.Entities
{
    public class IpAddress
    {
        public string AddressIP { get; set; } = string.Empty;

        public int IncorrectTrialsNumber { get; set; } = 0;

        public DateTime? LastBadLoginDate { get; set; } = null;

        public DateTime? LastSuccessLoginDate { get; set; } = null;

        public DateTime? LockDate { get; set; } = null;

        public ICollection<SignInRegister> SignInRegisters { get; set; } = new HashSet<SignInRegister>();
    }
}
