using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class profilepic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "obrazyProfilowe",
                columns: table => new
                {
                    id_uzytkownika = table.Column<int>(nullable: false),
                    obraz = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_obrazyProfilowe", x => x.id_uzytkownika);
                    table.ForeignKey(
                        name: "FK_obrazyProfilowe_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "obrazyProfilowe");
        }
    }
}
