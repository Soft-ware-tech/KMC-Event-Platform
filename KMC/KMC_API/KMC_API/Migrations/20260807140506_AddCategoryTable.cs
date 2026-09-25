using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMC_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });
                migrationBuilder.InsertData(
                    table: "Categories",
                    column: "Name",
                    values: new object[]
                {
                "Cultural", "Music", "Sports", "Workshop", "Exhibition",
                "Religious", "Community", "Food & Trade Fair", "Other"
        });
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
