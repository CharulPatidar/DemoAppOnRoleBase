using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApp.Migrations
{
    /// <inheritdoc />
    public partial class updateApplicationDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserSalt",
                table: "users",
                newName: "user_salt");

            migrationBuilder.RenameColumn(
                name: "UserHashedPassword",
                table: "users",
                newName: "user_hashed_password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_salt",
                table: "users",
                newName: "UserSalt");

            migrationBuilder.RenameColumn(
                name: "user_hashed_password",
                table: "users",
                newName: "UserHashedPassword");
        }
    }
}
