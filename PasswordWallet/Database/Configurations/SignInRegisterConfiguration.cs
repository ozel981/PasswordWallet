using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PasswordWallet.Database.Configurations
{
    public class SignInRegisterConfiguration : IEntityTypeConfiguration<Entities.SignInRegister>
    {
        public void Configure(EntityTypeBuilder<Entities.SignInRegister> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.User)
                .WithMany(u => u.SignInRegisters);

            builder.HasOne(r => r.IpAddress)
                .WithMany(a => a.SignInRegisters);

            builder.Property(r => r.Date)
                .IsRequired();
            
            builder.Property(r => r.IsCorrect)
                .IsRequired();

            builder.Property(r => r.Session)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Computer)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
