using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSHOP_TWO.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixBrandTranslationRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandTranslation_Brands_BrandId",
                table: "BrandTranslation");

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "BrandTranslation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BrandTranslation_Brands_BrandId",
                table: "BrandTranslation",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandTranslation_Brands_BrandId",
                table: "BrandTranslation");

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "BrandTranslation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandTranslation_Brands_BrandId",
                table: "BrandTranslation",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }
    }
}
