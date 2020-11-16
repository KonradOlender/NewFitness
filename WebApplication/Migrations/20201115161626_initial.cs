using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    imie = table.Column<string>(nullable: true),
                    login = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KategorieCwiczen",
                columns: table => new
                {
                    id_kategorii = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "varchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorieCwiczen", x => x.id_kategorii);
                });

            migrationBuilder.CreateTable(
                name: "KategorieSkladnikow",
                columns: table => new
                {
                    id_kategorii = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "varchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorieSkladnikow", x => x.id_kategorii);
                });

            migrationBuilder.CreateTable(
                name: "KategorieTreningow",
                columns: table => new
                {
                    id_kategorii = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "varchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorieTreningow", x => x.id_kategorii);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id_roli = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "varchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id_roli);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoriaUzytkownikow",
                columns: table => new
                {
                    id_uzytkownika = table.Column<int>(nullable: false),
                    data = table.Column<DateTime>(nullable: false),
                    waga = table.Column<double>(nullable: false),
                    wzrost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriaUzytkownikow", x => new { x.id_uzytkownika, x.data });
                    table.ForeignKey(
                        name: "FK_HistoriaUzytkownikow_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posilki",
                columns: table => new
                {
                    id_posilku = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "varchar(40)", nullable: false),
                    kalorie = table.Column<int>(nullable: false),
                    opis = table.Column<string>(type: "varchar(1000)", nullable: true),
                    id_uzytkownika = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posilki", x => x.id_posilku);
                    table.ForeignKey(
                        name: "FK_Posilki_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cwiczenia",
                columns: table => new
                {
                    id_cwiczenia = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "varchar(20)", nullable: false),
                    opis = table.Column<string>(type: "varchar(1000)", nullable: false),
                    spalone_kalorie = table.Column<int>(nullable: false),
                    id_kategorii = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cwiczenia", x => x.id_cwiczenia);
                    table.ForeignKey(
                        name: "FK_Cwiczenia_KategorieCwiczen_id_kategorii",
                        column: x => x.id_kategorii,
                        principalTable: "KategorieCwiczen",
                        principalColumn: "id_kategorii",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skladniki",
                columns: table => new
                {
                    id_skladnika = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    waga = table.Column<int>(nullable: false),
                    nazwa = table.Column<string>(type: "varchar(20)", nullable: false),
                    kalorie = table.Column<int>(nullable: false),
                    id_kategorii = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skladniki", x => x.id_skladnika);
                    table.ForeignKey(
                        name: "FK_Skladniki_KategorieSkladnikow_id_kategorii",
                        column: x => x.id_kategorii,
                        principalTable: "KategorieSkladnikow",
                        principalColumn: "id_kategorii",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Treningi",
                columns: table => new
                {
                    id_treningu = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "varchar(30)", nullable: false),
                    id_kategorii = table.Column<int>(nullable: false),
                    id_uzytkownika = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treningi", x => x.id_treningu);
                    table.ForeignKey(
                        name: "FK_Treningi_KategorieTreningow_id_kategorii",
                        column: x => x.id_kategorii,
                        principalTable: "KategorieTreningow",
                        principalColumn: "id_kategorii",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treningi_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "oceny",
                columns: table => new
                {
                    id_uzytkownika_oceniajacego = table.Column<int>(nullable: false),
                    id_uzytkownika_ocenianego = table.Column<int>(nullable: false),
                    id_roli = table.Column<int>(nullable: false),
                    ocena = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oceny", x => new { x.id_uzytkownika_ocenianego, x.id_uzytkownika_oceniajacego, x.id_roli });
                    table.ForeignKey(
                        name: "FK_oceny_Role_id_roli",
                        column: x => x.id_roli,
                        principalTable: "Role",
                        principalColumn: "id_roli",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_oceny_AspNetUsers_id_uzytkownika_oceniajacego",
                        column: x => x.id_uzytkownika_oceniajacego,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_oceny_AspNetUsers_id_uzytkownika_ocenianego",
                        column: x => x.id_uzytkownika_ocenianego,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUzytkownikow",
                columns: table => new
                {
                    id_uzytkownika = table.Column<int>(nullable: false),
                    id_roli = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUzytkownikow", x => new { x.id_roli, x.id_uzytkownika });
                    table.ForeignKey(
                        name: "FK_RoleUzytkownikow_Role_id_roli",
                        column: x => x.id_roli,
                        principalTable: "Role",
                        principalColumn: "id_roli",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUzytkownikow_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanowanePosilki",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_uzytkownika = table.Column<int>(nullable: false),
                    id_posilku = table.Column<int>(nullable: false),
                    data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanowanePosilki", x => x.id);
                    table.ForeignKey(
                        name: "FK_PlanowanePosilki_Posilki_id_posilku",
                        column: x => x.id_posilku,
                        principalTable: "Posilki",
                        principalColumn: "id_posilku",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanowanePosilki_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "posilekSzczegoly",
                columns: table => new
                {
                    id_posilku = table.Column<int>(nullable: false),
                    id_skladnika = table.Column<int>(nullable: false),
                    porcja = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posilekSzczegoly", x => new { x.id_posilku, x.id_skladnika });
                    table.ForeignKey(
                        name: "FK_posilekSzczegoly_Posilki_id_posilku",
                        column: x => x.id_posilku,
                        principalTable: "Posilki",
                        principalColumn: "id_posilku",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_posilekSzczegoly_Skladniki_id_skladnika",
                        column: x => x.id_skladnika,
                        principalTable: "Skladniki",
                        principalColumn: "id_skladnika",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanowaneTreningi",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_uzytkownika = table.Column<int>(nullable: false),
                    id_treningu = table.Column<int>(nullable: false),
                    data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanowaneTreningi", x => x.id);
                    table.ForeignKey(
                        name: "FK_PlanowaneTreningi_Treningi_id_treningu",
                        column: x => x.id_treningu,
                        principalTable: "Treningi",
                        principalColumn: "id_treningu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanowaneTreningi_AspNetUsers_id_uzytkownika",
                        column: x => x.id_uzytkownika,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "treningSzczegoly",
                columns: table => new
                {
                    id_treningu = table.Column<int>(nullable: false),
                    id_cwiczenia = table.Column<int>(nullable: false),
                    liczba_powtorzen = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_treningSzczegoly", x => new { x.id_treningu, x.id_cwiczenia });
                    table.ForeignKey(
                        name: "FK_treningSzczegoly_Cwiczenia_id_cwiczenia",
                        column: x => x.id_cwiczenia,
                        principalTable: "Cwiczenia",
                        principalColumn: "id_cwiczenia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_treningSzczegoly_Treningi_id_treningu",
                        column: x => x.id_treningu,
                        principalTable: "Treningi",
                        principalColumn: "id_treningu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cwiczenia_id_kategorii",
                table: "Cwiczenia",
                column: "id_kategorii");

            migrationBuilder.CreateIndex(
                name: "IX_oceny_id_roli",
                table: "oceny",
                column: "id_roli");

            migrationBuilder.CreateIndex(
                name: "IX_oceny_id_uzytkownika_oceniajacego",
                table: "oceny",
                column: "id_uzytkownika_oceniajacego");

            migrationBuilder.CreateIndex(
                name: "IX_PlanowanePosilki_id_posilku",
                table: "PlanowanePosilki",
                column: "id_posilku");

            migrationBuilder.CreateIndex(
                name: "IX_PlanowanePosilki_id_uzytkownika",
                table: "PlanowanePosilki",
                column: "id_uzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_PlanowaneTreningi_id_treningu",
                table: "PlanowaneTreningi",
                column: "id_treningu");

            migrationBuilder.CreateIndex(
                name: "IX_PlanowaneTreningi_id_uzytkownika",
                table: "PlanowaneTreningi",
                column: "id_uzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_posilekSzczegoly_id_skladnika",
                table: "posilekSzczegoly",
                column: "id_skladnika");

            migrationBuilder.CreateIndex(
                name: "IX_Posilki_id_uzytkownika",
                table: "Posilki",
                column: "id_uzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUzytkownikow_id_uzytkownika",
                table: "RoleUzytkownikow",
                column: "id_uzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_Skladniki_id_kategorii",
                table: "Skladniki",
                column: "id_kategorii");

            migrationBuilder.CreateIndex(
                name: "IX_Treningi_id_kategorii",
                table: "Treningi",
                column: "id_kategorii");

            migrationBuilder.CreateIndex(
                name: "IX_Treningi_id_uzytkownika",
                table: "Treningi",
                column: "id_uzytkownika");

            migrationBuilder.CreateIndex(
                name: "IX_treningSzczegoly_id_cwiczenia",
                table: "treningSzczegoly",
                column: "id_cwiczenia");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HistoriaUzytkownikow");

            migrationBuilder.DropTable(
                name: "oceny");

            migrationBuilder.DropTable(
                name: "PlanowanePosilki");

            migrationBuilder.DropTable(
                name: "PlanowaneTreningi");

            migrationBuilder.DropTable(
                name: "posilekSzczegoly");

            migrationBuilder.DropTable(
                name: "RoleUzytkownikow");

            migrationBuilder.DropTable(
                name: "treningSzczegoly");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Posilki");

            migrationBuilder.DropTable(
                name: "Skladniki");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Cwiczenia");

            migrationBuilder.DropTable(
                name: "Treningi");

            migrationBuilder.DropTable(
                name: "KategorieSkladnikow");

            migrationBuilder.DropTable(
                name: "KategorieCwiczen");

            migrationBuilder.DropTable(
                name: "KategorieTreningow");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
