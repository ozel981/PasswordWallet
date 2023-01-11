using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PasswordWallet.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Entities.User>
    {
        public void Configure(EntityTypeBuilder<Entities.User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(u => u.PublicKeyJsonStr)
                .IsRequired()
                .HasMaxLength(2048);

            builder.Property(u => u.PrivateKeyJsonStr)
                .IsRequired()
                .HasMaxLength(4096);

            builder.Property(u => u.Salt)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasMany(u => u.Passwords)
               .WithOne(p => p.User)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.SignInRegisters)
               .WithOne(r => r.User)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.SharedPasswords)
               .WithOne(sp => sp.User)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
