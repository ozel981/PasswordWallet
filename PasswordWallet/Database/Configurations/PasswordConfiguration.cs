using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PasswordWallet.Database.Configurations
{
    public class PasswordConfiguration : IEntityTypeConfiguration<Entities.Password>
    {
        public void Configure(EntityTypeBuilder<Entities.Password> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PasswordText)
                .IsRequired()
                .HasMaxLength(512);

            builder.HasOne(p => p.User)
                .WithMany(u => u.Passwords);

            builder.Property(p => p.WebAdderss)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(p => p.Login)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasMany(u => u.SharedPasswords)
               .WithOne(sp => sp.Password)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
