using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spacemeet.Migrations
{
    public partial class AdduserNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "phoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "companyName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "companyName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Users",
                newName: "Username");
        }
    }
}
