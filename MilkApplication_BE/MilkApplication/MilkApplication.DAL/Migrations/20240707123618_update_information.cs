using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkApplication.DAL.Migrations
{
    /// <inheritdoc />
<<<<<<<< HEAD:MilkApplication_BE/MilkApplication/MilkApplication.DAL/Migrations/20240707083840_fix_1.cs
    public partial class fix_1 : Migration
========
    public partial class update_information : Migration
>>>>>>>> main:MilkApplication_BE/MilkApplication/MilkApplication.DAL/Migrations/20240707123618_update_information.cs
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
<<<<<<<< HEAD:MilkApplication_BE/MilkApplication/MilkApplication.DAL/Migrations/20240707083840_fix_1.cs

========
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Comments");
>>>>>>>> main:MilkApplication_BE/MilkApplication/MilkApplication.DAL/Migrations/20240707123618_update_information.cs
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
<<<<<<<< HEAD:MilkApplication_BE/MilkApplication/MilkApplication.DAL/Migrations/20240707083840_fix_1.cs

========
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
>>>>>>>> main:MilkApplication_BE/MilkApplication/MilkApplication.DAL/Migrations/20240707123618_update_information.cs
        }
    }
}
