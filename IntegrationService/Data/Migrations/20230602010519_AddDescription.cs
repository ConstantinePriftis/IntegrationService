using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Data.Migrations
{
    public partial class AddDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "UserId",
                keyValue: "5bb3e9b2-7291-408c-8a7a-ed96008bc9af");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "53013d85-d2e0-4e38-85c0-6d27e7c02226");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductCharacteristics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue", "Id" },
                values: new object[] { "d6a5fb71-779b-411f-85d4-447026bccc22", "IsAdmin", "true", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "70e2f2da-4849-4a66-803f-1aa9b901d2df", 0, "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8", "kpriftis@sklavenitis.com", true, "priftis", false, null, "Kostas", "kpriftis@sklavenitis.com", "kpriftis@sklavenitis.com", "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==", null, false, "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF", false, "kpriftis@sklavenitis.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "UserId",
                keyValue: "d6a5fb71-779b-411f-85d4-447026bccc22");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "70e2f2da-4849-4a66-803f-1aa9b901d2df");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductCharacteristics");

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue", "Id" },
                values: new object[] { "5bb3e9b2-7291-408c-8a7a-ed96008bc9af", "IsAdmin", "true", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "53013d85-d2e0-4e38-85c0-6d27e7c02226", 0, "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8", "kpriftis@sklavenitis.com", true, "priftis", false, null, "Kostas", "kpriftis@sklavenitis.com", "kpriftis@sklavenitis.com", "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==", null, false, "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF", false, "kpriftis@sklavenitis.com" });
        }
    }
}
