using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedChannelDifference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "UserId",
                keyValue: "d2d05bcd-dc89-4382-b30d-0a622749c96d");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2e6fc85d-41ae-4e28-b9c7-12d053e3c5ab");

            migrationBuilder.CreateTable(
                name: "ChannelDifferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductStoresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelDifferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelDifferences_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelDifferences_ProductStores_ProductStoresId",
                        column: x => x.ProductStoresId,
                        principalTable: "ProductStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue", "Id" },
                values: new object[] { "aeadab6c-4592-43e9-a7a7-e0a687feeefb", "IsAdmin", "true", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2262b990-ffaf-47c2-bf65-0c8aa2154c85", 0, "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8", "kpriftis@sklavenitis.com", true, "priftis", false, null, "Kostas", "kpriftis@sklavenitis.com", "kpriftis@sklavenitis.com", "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==", null, false, "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF", false, "kpriftis@sklavenitis.com" });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelDifferences_ChannelId",
                table: "ChannelDifferences",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelDifferences_ProductStoresId",
                table: "ChannelDifferences",
                column: "ProductStoresId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelDifferences");

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "UserId",
                keyValue: "aeadab6c-4592-43e9-a7a7-e0a687feeefb");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2262b990-ffaf-47c2-bf65-0c8aa2154c85");

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue", "Id" },
                values: new object[] { "d2d05bcd-dc89-4382-b30d-0a622749c96d", "IsAdmin", "true", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2e6fc85d-41ae-4e28-b9c7-12d053e3c5ab", 0, "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8", "kpriftis@sklavenitis.com", true, "priftis", false, null, "Kostas", "kpriftis@sklavenitis.com", "kpriftis@sklavenitis.com", "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==", null, false, "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF", false, "kpriftis@sklavenitis.com" });
        }
    }
}
