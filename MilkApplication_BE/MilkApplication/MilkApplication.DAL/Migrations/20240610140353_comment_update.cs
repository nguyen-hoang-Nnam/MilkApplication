using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class comment_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_Id",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_Id",
                table: "Comment",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_Id",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_Id",
                table: "Comment",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
