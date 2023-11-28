using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Data.Migrations
{
    public partial class ChangeImportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductEnabled",
                table: "Imports");

            migrationBuilder.RenameColumn(
                name: "ProductDescription",
                table: "Imports",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Imports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Digital",
                table: "Imports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Efood",
                table: "Imports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Imports");

            migrationBuilder.DropColumn(
                name: "Digital",
                table: "Imports");

            migrationBuilder.DropColumn(
                name: "Efood",
                table: "Imports");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Imports",
                newName: "ProductDescription");

            migrationBuilder.AddColumn<bool>(
                name: "ProductEnabled",
                table: "Imports",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
