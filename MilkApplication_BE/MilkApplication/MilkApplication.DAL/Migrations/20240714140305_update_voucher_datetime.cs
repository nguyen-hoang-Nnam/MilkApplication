using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_voucher_datetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "Vouchers",
                newName: "dateTo");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateFrom",
                table: "Vouchers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateFrom",
                table: "Vouchers");

            migrationBuilder.RenameColumn(
                name: "dateTo",
                table: "Vouchers",
                newName: "date");
        }
    }
}
