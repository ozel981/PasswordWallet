using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PasswordWallet.Database.Configurations
{
    public class SharedPasswordConfiguration : IEntityTypeConfiguration<Entities.SharedPassword>
    {
        public void Configure(EntityTypeBuilder<Entities.SharedPassword> builder)
        {
            builder.HasKey(sp => sp.Id);

            builder.Property(sp => sp.PasswordText)
                .IsRequired()
                .HasMaxLength(512);

            builder.HasOne(sp => sp.User)
                .WithMany(u => u.SharedPasswords);

            builder.HasOne(sp => sp.Password)
                .WithMany(p => p.SharedPasswords);

            builder.Property(sp => sp.IsNew)
                .IsRequired();
        }
    }
}
