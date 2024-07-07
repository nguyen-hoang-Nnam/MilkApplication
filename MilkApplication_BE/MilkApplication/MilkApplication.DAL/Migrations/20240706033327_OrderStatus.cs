using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkApplication.DAL.Migrations
{
    /// <inheritdoc />
<<<<<<<< HEAD:MilkApplication_BE/MilkApplication/MilkApplication.DAL/Migrations/20240706033327_OrderStatus.cs
    public partial class OrderStatus : Migration
========
    public partial class update_images : Migration
>>>>>>>> main:MilkApplication_BE/MilkApplication/MilkApplication.DAL/Migrations/20240707142439_update_images.cs
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");
        }
    }
}
