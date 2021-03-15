using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class chatnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameOne",
                table: "chats",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameTwo",
                table: "chats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameOne",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "NameTwo",
                table: "chats");
        }
    }
}
