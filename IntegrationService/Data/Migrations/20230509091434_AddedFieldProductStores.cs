using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFieldProductStores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "UserId",
                keyValue: "aeadab6c-4592-43e9-a7a7-e0a687feeefb");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2262b990-ffaf-47c2-bf65-0c8aa2154c85");

            migrationBuilder.CreateTable(
                name: "FieldProductStores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductStoresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldProductStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldProductStores_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldProductStores_ProductStores_ProductStoresId",
                        column: x => x.ProductStoresId,
                        principalTable: "ProductStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue", "Id" },
                values: new object[] { "6a7fcfa3-edab-4b58-ab1d-7502842742de", "IsAdmin", "true", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "04e73975-737b-4b73-96af-1ed03438f8c2", 0, "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8", "kpriftis@sklavenitis.com", true, "priftis", false, null, "Kostas", "kpriftis@sklavenitis.com", "kpriftis@sklavenitis.com", "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==", null, false, "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF", false, "kpriftis@sklavenitis.com" });

            migrationBuilder.CreateIndex(
                name: "IX_FieldProductStores_FieldId",
                table: "FieldProductStores",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldProductStores_ProductStoresId",
                table: "FieldProductStores",
                column: "ProductStoresId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldProductStores");

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "UserId",
                keyValue: "6a7fcfa3-edab-4b58-ab1d-7502842742de");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "04e73975-737b-4b73-96af-1ed03438f8c2");

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue", "Id" },
                values: new object[] { "aeadab6c-4592-43e9-a7a7-e0a687feeefb", "IsAdmin", "true", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2262b990-ffaf-47c2-bf65-0c8aa2154c85", 0, "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8", "kpriftis@sklavenitis.com", true, "priftis", false, null, "Kostas", "kpriftis@sklavenitis.com", "kpriftis@sklavenitis.com", "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==", null, false, "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF", false, "kpriftis@sklavenitis.com" });
        }
    }
}
