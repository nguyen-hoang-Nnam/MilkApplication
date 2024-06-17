using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class location_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "locationId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    locationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    locationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.locationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_locationId",
                table: "Products",
                column: "locationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Locations_locationId",
                table: "Products",
                column: "locationId",
                principalTable: "Locations",
                principalColumn: "locationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Locations_locationId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Products_locationId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "locationId",
                table: "Products");
        }
    }
}
