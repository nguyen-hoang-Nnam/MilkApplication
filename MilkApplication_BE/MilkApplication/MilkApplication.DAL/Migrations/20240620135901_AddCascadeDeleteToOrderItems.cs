using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_orderId",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_orderId",
                table: "OrderItems",
                column: "orderId",
                principalTable: "Orders",
                principalColumn: "orderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_orderId",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_orderId",
                table: "OrderItems",
                column: "orderId",
                principalTable: "Orders",
                principalColumn: "orderId");
        }
    }
}
