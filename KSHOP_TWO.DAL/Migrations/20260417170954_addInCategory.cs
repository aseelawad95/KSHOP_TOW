using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSHOP_TWO.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addInCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Categories");
        }
    }
}
