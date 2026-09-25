using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMC_API.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizerIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Organizers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Organizers");
        }
    }
}
