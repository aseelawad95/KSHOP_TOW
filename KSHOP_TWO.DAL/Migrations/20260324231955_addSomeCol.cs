using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSHOP_TWO.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addSomeCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeRestPassword",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordRestCodeExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeRestPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordRestCodeExpiry",
                table: "Users");
        }
    }
}
