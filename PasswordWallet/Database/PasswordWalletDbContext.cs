using Microsoft.EntityFrameworkCore;
using PasswordWallet.Database;

namespace PasswordWallet.Database
{
    public class PasswordWalletDbContext : DbContext
    {
        public PasswordWalletDbContext(DbContextOptions<PasswordWalletDbContext> options) : base(options) { }
        public PasswordWalletDbContext() : base() { }
        public virtual DbSet<Entities.User> Users { get; set; }
        public virtual DbSet<Entities.Password> Passwords { get; set; }
        public virtual DbSet<Entities.IpAddress> IpAddresses { get; set; }
        public virtual DbSet<Entities.SignInRegister> SignInRegisters { get; set; }
        public virtual DbSet<Entities.SharedPassword> SharedPasswords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PasswordConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.IpAddressConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SignInRegisterConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SharedPasswordConfiguration());
        }
    }
}
