using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Data.Migrations
{
    public partial class RemoveStoreFromFieldProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldProducts_Stores_StoreId",
                table: "FieldProducts");

            migrationBuilder.DropIndex(
                name: "IX_FieldProducts_StoreId",
                table: "FieldProducts");

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "UserId",
                keyValue: "b37ca9fb-1721-44c1-aebb-bc8a2878a69c");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2a49a6ce-6f49-40fd-8533-5fa9f9849490");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "FieldProducts");

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue", "Id" },
                values: new object[] { "5b2fa2ef-ea6b-4ce6-9039-1e2fd5d9b7ed", "IsAdmin", "true", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "186dea4f-d53c-427e-8dc4-e6a2a69ce525", 0, "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8", "kpriftis@sklavenitis.com", true, "priftis", false, null, "Kostas", "kpriftis@sklavenitis.com", "kpriftis@sklavenitis.com", "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==", null, false, "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF", false, "kpriftis@sklavenitis.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "UserId",
                keyValue: "5b2fa2ef-ea6b-4ce6-9039-1e2fd5d9b7ed");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "186dea4f-d53c-427e-8dc4-e6a2a69ce525");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "FieldProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue", "Id" },
                values: new object[] { "b37ca9fb-1721-44c1-aebb-bc8a2878a69c", "IsAdmin", "true", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2a49a6ce-6f49-40fd-8533-5fa9f9849490", 0, "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8", "kpriftis@sklavenitis.com", true, "priftis", false, null, "Kostas", "kpriftis@sklavenitis.com", "kpriftis@sklavenitis.com", "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==", null, false, "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF", false, "kpriftis@sklavenitis.com" });

            migrationBuilder.CreateIndex(
                name: "IX_FieldProducts_StoreId",
                table: "FieldProducts",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldProducts_Stores_StoreId",
                table: "FieldProducts",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }
    }
}
