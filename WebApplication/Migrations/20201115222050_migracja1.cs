using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class migracja1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "KategorieCwiczen",
                columns: new[] { "id_kategorii", "nazwa" },
                values: new object[] { 1, "inne" });

            migrationBuilder.InsertData(
                table: "KategorieSkladnikow",
                columns: new[] { "id_kategorii", "nazwa" },
                values: new object[] { 1, "inne" });

            migrationBuilder.InsertData(
                table: "KategorieTreningow",
                columns: new[] { "id_kategorii", "nazwa" },
                values: new object[] { 1, "inne" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "id_roli", "nazwa" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "trener" },
                    { 3, "dietetyk" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KategorieCwiczen",
                keyColumn: "id_kategorii",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KategorieSkladnikow",
                keyColumn: "id_kategorii",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KategorieTreningow",
                keyColumn: "id_kategorii",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "id_roli",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "id_roli",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "id_roli",
                keyValue: 3);
        }
    }
}
