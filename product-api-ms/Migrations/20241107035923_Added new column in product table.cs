using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace product_api_ms.Migrations
{
    /// <inheritdoc />
    public partial class Addednewcolumninproducttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Originated",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Originated",
                table: "Products");
        }
    }
}
