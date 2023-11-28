using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Data.Migrations
{
    public partial class NewAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue", "Id" },
                values: new object[] { "7ec0b9e0-99da-4d09-8ebc-8c30a063ef17", "IsAdmin", "true", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "53cab02a-8a6f-4fba-802b-d68382384125", 0, "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8", "kpriftis@sklavenitis.com", true, "priftis", false, null, "Kostas", "kpriftis@sklavenitis.com", "kpriftis@sklavenitis.com", "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==", null, false, "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF", false, "kpriftis@sklavenitis.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "UserId",
                keyValue: "7ec0b9e0-99da-4d09-8ebc-8c30a063ef17");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "53cab02a-8a6f-4fba-802b-d68382384125");
        }
    }
}
