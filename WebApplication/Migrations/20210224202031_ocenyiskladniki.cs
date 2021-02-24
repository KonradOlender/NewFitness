using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class ocenyiskladniki : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bialko",
                table: "Skladniki",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "tluszcze",
                table: "Skladniki",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "weglowodany",
                table: "Skladniki",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OcenyPosilkow",
                columns: table => new
                {
                    id_uzytkownika = table.Column<int>(nullable: false),
                    id_posilku = table.Column<int>(nullable: false),
                    ocena = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcenyPosilkow", x => new { x.id_posilku, x.id_uzytkownika });
                    table.ForeignKey(
                        name: "FK_OcenyPosilkow_Posilki_id_posilku",
                        column: x => x.id_posilku,
                        principalTable: "Posilki",
                        principalColumn: "id_posilku",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OcenyPosilkow_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OcenyTreningow",
                columns: table => new
                {
                    id_uzytkownika = table.Column<int>(nullable: false),
                    id_treningu = table.Column<int>(nullable: false),
                    ocena = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcenyTreningow", x => new { x.id_treningu, x.id_uzytkownika });
                    table.ForeignKey(
                        name: "FK_OcenyTreningow_Treningi_id_treningu",
                        column: x => x.id_treningu,
                        principalTable: "Treningi",
                        principalColumn: "id_treningu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OcenyTreningow_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OcenyPosilkow_id_uzytkownika",
                table: "OcenyPosilkow",
                column: "id_uzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_OcenyTreningow_id_uzytkownika",
                table: "OcenyTreningow",
                column: "id_uzytkownika");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OcenyPosilkow");

            migrationBuilder.DropTable(
                name: "OcenyTreningow");

            migrationBuilder.DropColumn(
                name: "bialko",
                table: "Skladniki");

            migrationBuilder.DropColumn(
                name: "tluszcze",
                table: "Skladniki");

            migrationBuilder.DropColumn(
                name: "weglowodany",
                table: "Skladniki");
        }
    }
}
