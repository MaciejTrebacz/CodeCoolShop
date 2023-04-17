using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codecool.CodecoolShop.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Currency", "DefaultPrice", "Description", "Name", "ProductCategory", "SupplierId" },
                values: new object[] { 1, "USD", 479.0m, null, "Lenovo IdeaPad Miix 700", 0, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
