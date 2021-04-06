using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class formats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "format",
                table: "obrazyTreningow",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "format",
                table: "obrazyProfilowe",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "format",
                table: "obrazyPosilkow",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "format",
                table: "obrazyTreningow");

            migrationBuilder.DropColumn(
                name: "format",
                table: "obrazyProfilowe");

            migrationBuilder.DropColumn(
                name: "format",
                table: "obrazyPosilkow");
        }
    }
}
