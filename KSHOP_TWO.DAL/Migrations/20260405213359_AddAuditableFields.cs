using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSHOP_TWO.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "logo",
                table: "Brands",
                newName: "Logo");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "BrandTranslation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "BrandTranslation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BrandTranslation_BrandId",
                table: "BrandTranslation",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandTranslation_Brands_BrandId",
                table: "BrandTranslation",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandTranslation_Brands_BrandId",
                table: "BrandTranslation");

            migrationBuilder.DropIndex(
                name: "IX_BrandTranslation_BrandId",
                table: "BrandTranslation");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "BrandTranslation");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "BrandTranslation");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Brands",
                newName: "logo");
        }
    }
}
