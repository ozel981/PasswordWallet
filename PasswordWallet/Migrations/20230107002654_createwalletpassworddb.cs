using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordWallet.Migrations
{
    /// <inheritdoc />
    public partial class createwalletpassworddb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IpAddresses",
                columns: table => new
                {
                    AddressIP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IncorrectTrialsNumber = table.Column<int>(type: "int", nullable: false),
                    LastBadLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSuccessLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpAddresses", x => x.AddressIP);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsPasswordKeptAsHash = table.Column<bool>(type: "bit", nullable: false),
                    PublicKeyJsonStr = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    PrivateKeyJsonStr = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passwords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PasswordText = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    WebAdderss = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passwords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passwords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignInRegisters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IpAddressAddressIP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    Session = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Computer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignInRegisters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignInRegisters_IpAddresses_IpAddressAddressIP",
                        column: x => x.IpAddressAddressIP,
                        principalTable: "IpAddresses",
                        principalColumn: "AddressIP",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignInRegisters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedPasswords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PasswordId = table.Column<int>(type: "int", nullable: false),
                    PasswordText = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedPasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedPasswords_Passwords_PasswordId",
                        column: x => x.PasswordId,
                        principalTable: "Passwords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SharedPasswords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_UserId",
                table: "Passwords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPasswords_PasswordId",
                table: "SharedPasswords",
                column: "PasswordId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPasswords_UserId",
                table: "SharedPasswords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SignInRegisters_IpAddressAddressIP",
                table: "SignInRegisters",
                column: "IpAddressAddressIP");

            migrationBuilder.CreateIndex(
                name: "IX_SignInRegisters_UserId",
                table: "SignInRegisters",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedPasswords");

            migrationBuilder.DropTable(
                name: "SignInRegisters");

            migrationBuilder.DropTable(
                name: "Passwords");

            migrationBuilder.DropTable(
                name: "IpAddresses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
