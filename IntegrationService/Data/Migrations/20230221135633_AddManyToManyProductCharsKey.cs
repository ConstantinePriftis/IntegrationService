using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Data.Migrations
{
    public partial class AddManyToManyProductCharsKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCharacteristicStores",
                table: "ProductCharacteristicStores");

            migrationBuilder.DropIndex(
                name: "IX_ProductCharacteristicStores_ProductCharacteristicId",
                table: "ProductCharacteristicStores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCharacteristicStores",
                table: "ProductCharacteristicStores",
                columns: new[] { "ProductCharacteristicId", "StoreId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCharacteristicStores",
                table: "ProductCharacteristicStores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCharacteristicStores",
                table: "ProductCharacteristicStores",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCharacteristicStores_ProductCharacteristicId",
                table: "ProductCharacteristicStores",
                column: "ProductCharacteristicId");
        }
    }
}
