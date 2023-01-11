using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PasswordWallet.Database.Configurations
{
    public class IpAddressConfiguration : IEntityTypeConfiguration<Entities.IpAddress>
    {
        public void Configure(EntityTypeBuilder<Entities.IpAddress> builder)
        {
            builder.HasKey(a => a.AddressIP);

            builder.Property(a => a.IncorrectTrialsNumber)
                .IsRequired();

            builder.Property(a => a.LastSuccessLoginDate);

            builder.Property(a => a.LastBadLoginDate);

            builder.Property(a => a.LockDate);

            builder.HasMany(a => a.SignInRegisters)
               .WithOne(r => r.IpAddress)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
