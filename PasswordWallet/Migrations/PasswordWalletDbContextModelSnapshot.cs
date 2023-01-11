﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PasswordWallet.Database;

#nullable disable

namespace PasswordWallet.Migrations
{
    [DbContext(typeof(PasswordWalletDbContext))]
    partial class PasswordWalletDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc.2.22472.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PasswordWallet.Database.Entities.IpAddress", b =>
                {
                    b.Property<string>("AddressIP")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("IncorrectTrialsNumber")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastBadLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastSuccessLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LockDate")
                        .HasColumnType("datetime2");

                    b.HasKey("AddressIP");

                    b.ToTable("IpAddresses");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.Password", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("PasswordText")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("WebAdderss")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Passwords");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.SharedPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsNew")
                        .HasColumnType("bit");

                    b.Property<int>("PasswordId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordText")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PasswordId");

                    b.HasIndex("UserId");

                    b.ToTable("SharedPasswords");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.SignInRegister", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Computer")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("IpAddressAddressIP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<string>("Session")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IpAddressAddressIP");

                    b.HasIndex("UserId");

                    b.ToTable("SignInRegisters");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsPasswordKeptAsHash")
                        .HasColumnType("bit");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("PrivateKeyJsonStr")
                        .IsRequired()
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicKeyJsonStr")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.Password", b =>
                {
                    b.HasOne("PasswordWallet.Database.Entities.User", "User")
                        .WithMany("Passwords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.SharedPassword", b =>
                {
                    b.HasOne("PasswordWallet.Database.Entities.Password", "Password")
                        .WithMany("SharedPasswords")
                        .HasForeignKey("PasswordId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PasswordWallet.Database.Entities.User", "User")
                        .WithMany("SharedPasswords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Password");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.SignInRegister", b =>
                {
                    b.HasOne("PasswordWallet.Database.Entities.IpAddress", "IpAddress")
                        .WithMany("SignInRegisters")
                        .HasForeignKey("IpAddressAddressIP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PasswordWallet.Database.Entities.User", "User")
                        .WithMany("SignInRegisters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("IpAddress");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.IpAddress", b =>
                {
                    b.Navigation("SignInRegisters");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.Password", b =>
                {
                    b.Navigation("SharedPasswords");
                });

            modelBuilder.Entity("PasswordWallet.Database.Entities.User", b =>
                {
                    b.Navigation("Passwords");

                    b.Navigation("SharedPasswords");

                    b.Navigation("SignInRegisters");
                });
#pragma warning restore 612, 618
        }
    }
}
