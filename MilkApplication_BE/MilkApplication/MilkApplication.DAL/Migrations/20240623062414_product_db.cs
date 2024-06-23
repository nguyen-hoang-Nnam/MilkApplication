using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class product_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "discountPercent",
                table: "Products",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "discountPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discountPercent",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "discountPrice",
                table: "Products");
        }
    }
}
