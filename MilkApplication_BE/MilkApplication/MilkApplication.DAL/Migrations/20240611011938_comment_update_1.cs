using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class comment_update_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_Id",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Products_productId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_productId",
                table: "Comments",
                newName: "IX_Comments_productId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_Id",
                table: "Comments",
                newName: "IX_Comments_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "commentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_Id",
                table: "Comments",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Products_productId",
                table: "Comments",
                column: "productId",
                principalTable: "Products",
                principalColumn: "productId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_Id",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Products_productId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_productId",
                table: "Comment",
                newName: "IX_Comment_productId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_Id",
                table: "Comment",
                newName: "IX_Comment_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "commentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_Id",
                table: "Comment",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Products_productId",
                table: "Comment",
                column: "productId",
                principalTable: "Products",
                principalColumn: "productId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
