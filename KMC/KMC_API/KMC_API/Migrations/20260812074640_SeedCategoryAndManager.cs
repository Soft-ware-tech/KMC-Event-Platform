using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMC_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoryAndManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "ManagerId", "CreatedDate", "FullName", "PasswordHash", "Username" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "KMC Admin", "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Managers",
                keyColumn: "ManagerId",
                keyValue: 1);
        }
    }
}
