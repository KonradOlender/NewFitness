using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class prosb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProsbyOUprawnienia",
                columns: table => new
                {
                    id_uzytkownika = table.Column<int>(nullable: false),
                    id_roli = table.Column<int>(nullable: false),
                    prosba_pisemna = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProsbyOUprawnienia", x => new { x.id_uzytkownika, x.id_roli });
                    table.ForeignKey(
                        name: "FK_ProsbyOUprawnienia_Role_id_roli",
                        column: x => x.id_roli,
                        principalTable: "Role",
                        principalColumn: "id_roli",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProsbyOUprawnienia_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProsbyOUprawnienia_id_roli",
                table: "ProsbyOUprawnienia",
                column: "id_roli");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProsbyOUprawnienia");
        }
    }
}
