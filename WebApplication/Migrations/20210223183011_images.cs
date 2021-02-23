using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "obrazyPosilkow",
                columns: table => new
                {
                    id_obrazu = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    obraz = table.Column<byte[]>(nullable: false),
                    id_posilku = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_obrazyPosilkow", x => x.id_obrazu);
                    table.ForeignKey(
                        name: "FK_obrazyPosilkow_Posilki_id_posilku",
                        column: x => x.id_posilku,
                        principalTable: "Posilki",
                        principalColumn: "id_posilku",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "obrazyTreningow",
                columns: table => new
                {
                    id_obrazu = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    obraz = table.Column<byte[]>(nullable: false),
                    id_treningu = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_obrazyTreningow", x => x.id_obrazu);
                    table.ForeignKey(
                        name: "FK_obrazyTreningow_Treningi_id_treningu",
                        column: x => x.id_treningu,
                        principalTable: "Treningi",
                        principalColumn: "id_treningu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_obrazyPosilkow_id_posilku",
                table: "obrazyPosilkow",
                column: "id_posilku");

            migrationBuilder.CreateIndex(
                name: "IX_obrazyTreningow_id_treningu",
                table: "obrazyTreningow",
                column: "id_treningu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "obrazyPosilkow");

            migrationBuilder.DropTable(
                name: "obrazyTreningow");
        }
    }
}
