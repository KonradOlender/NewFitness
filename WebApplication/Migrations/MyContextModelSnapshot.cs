﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication.Data;

namespace WebApplication.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WebApplication.Areas.Identity.Data.Uzytkownik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("imie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("limit")
                        .HasColumnType("int");

                    b.Property<string>("login")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WebApplication.Models.Cwiczenie", b =>
                {
                    b.Property<int>("id_cwiczenia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("id_kategorii")
                        .HasColumnType("int");

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("opis")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<int>("spalone_kalorie")
                        .HasColumnType("int");

                    b.HasKey("id_cwiczenia");

                    b.HasIndex("id_kategorii");

                    b.ToTable("Cwiczenia");
                });

            modelBuilder.Entity("WebApplication.Models.HistoriaUzytkownika", b =>
                {
                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.Property<DateTime>("data")
                        .HasColumnType("datetime2");

                    b.Property<double>("waga")
                        .HasColumnType("float");

                    b.Property<int>("wzrost")
                        .HasColumnType("int");

                    b.HasKey("id_uzytkownika", "data");

                    b.ToTable("HistoriaUzytkownikow");
                });

            modelBuilder.Entity("WebApplication.Models.KategoriaCwiczenia", b =>
                {
                    b.Property<int>("id_kategorii")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.HasKey("id_kategorii");

                    b.ToTable("KategorieCwiczen");

                    b.HasData(
                        new
                        {
                            id_kategorii = 1,
                            nazwa = "inne"
                        });
                });

            modelBuilder.Entity("WebApplication.Models.KategoriaSkladnikow", b =>
                {
                    b.Property<int>("id_kategorii")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.HasKey("id_kategorii");

                    b.ToTable("KategorieSkladnikow");

                    b.HasData(
                        new
                        {
                            id_kategorii = 1,
                            nazwa = "inne"
                        });
                });

            modelBuilder.Entity("WebApplication.Models.KategoriaTreningu", b =>
                {
                    b.Property<int>("id_kategorii")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.HasKey("id_kategorii");

                    b.ToTable("KategorieTreningow");

                    b.HasData(
                        new
                        {
                            id_kategorii = 1,
                            nazwa = "inne"
                        });
                });

            modelBuilder.Entity("WebApplication.Models.ObrazProfilowe", b =>
                {
                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.Property<byte[]>("obraz")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("id_uzytkownika");

                    b.ToTable("obrazyProfilowe");
                });

            modelBuilder.Entity("WebApplication.Models.ObrazyPosilku", b =>
                {
                    b.Property<int>("id_obrazu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("id_posilku")
                        .HasColumnType("int");

                    b.Property<byte[]>("obraz")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("id_obrazu");

                    b.HasIndex("id_posilku");

                    b.ToTable("obrazyPosilkow");
                });

            modelBuilder.Entity("WebApplication.Models.ObrazyTreningu", b =>
                {
                    b.Property<int>("id_obrazu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("id_treningu")
                        .HasColumnType("int");

                    b.Property<byte[]>("obraz")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("id_obrazu");

                    b.HasIndex("id_treningu");

                    b.ToTable("obrazyTreningow");
                });

            modelBuilder.Entity("WebApplication.Models.Ocena", b =>
                {
                    b.Property<int>("id_uzytkownika_ocenianego")
                        .HasColumnType("int");

                    b.Property<int>("id_uzytkownika_oceniajacego")
                        .HasColumnType("int");

                    b.Property<int>("id_roli")
                        .HasColumnType("int");

                    b.Property<double>("ocena")
                        .HasColumnType("float");

                    b.HasKey("id_uzytkownika_ocenianego", "id_uzytkownika_oceniajacego", "id_roli");

                    b.HasIndex("id_roli");

                    b.HasIndex("id_uzytkownika_oceniajacego");

                    b.ToTable("oceny");
                });

            modelBuilder.Entity("WebApplication.Models.OcenaPosilku", b =>
                {
                    b.Property<int>("id_posilku")
                        .HasColumnType("int");

                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.Property<double>("ocena")
                        .HasColumnType("float");

                    b.HasKey("id_posilku", "id_uzytkownika");

                    b.HasIndex("id_uzytkownika");

                    b.ToTable("OcenyPosilkow");
                });

            modelBuilder.Entity("WebApplication.Models.OcenaTreningu", b =>
                {
                    b.Property<int>("id_treningu")
                        .HasColumnType("int");

                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.Property<double>("ocena")
                        .HasColumnType("float");

                    b.HasKey("id_treningu", "id_uzytkownika");

                    b.HasIndex("id_uzytkownika");

                    b.ToTable("OcenyTreningow");
                });

            modelBuilder.Entity("WebApplication.Models.PlanowaniePosilkow", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("data")
                        .HasColumnType("datetime2");

                    b.Property<int>("id_posilku")
                        .HasColumnType("int");

                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("id_posilku");

                    b.HasIndex("id_uzytkownika");

                    b.ToTable("PlanowanePosilki");
                });

            modelBuilder.Entity("WebApplication.Models.PlanowanieTreningow", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("data")
                        .HasColumnType("datetime2");

                    b.Property<string>("dzien")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("id_treningu")
                        .HasColumnType("int");

                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("id_treningu");

                    b.HasIndex("id_uzytkownika");

                    b.ToTable("PlanowaneTreningi");
                });

            modelBuilder.Entity("WebApplication.Models.Posilek", b =>
                {
                    b.Property<int>("id_posilku")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.Property<int>("kalorie")
                        .HasColumnType("int");

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasColumnType("varchar(40)");

                    b.Property<string>("opis")
                        .HasColumnType("varchar(1000)");

                    b.HasKey("id_posilku");

                    b.HasIndex("id_uzytkownika");

                    b.ToTable("Posilki");
                });

            modelBuilder.Entity("WebApplication.Models.PosilekSzczegoly", b =>
                {
                    b.Property<int>("id_posilku")
                        .HasColumnType("int");

                    b.Property<int>("id_skladnika")
                        .HasColumnType("int");

                    b.Property<int>("porcja")
                        .HasColumnType("int");

                    b.HasKey("id_posilku", "id_skladnika");

                    b.HasIndex("id_skladnika");

                    b.ToTable("posilekSzczegoly");
                });

            modelBuilder.Entity("WebApplication.Models.ProsbyOUprawnienia", b =>
                {
                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.Property<int>("id_roli")
                        .HasColumnType("int");

                    b.Property<string>("prosba_pisemna")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_uzytkownika", "id_roli");

                    b.HasIndex("id_roli");

                    b.ToTable("ProsbyOUprawnienia");
                });

            modelBuilder.Entity("WebApplication.Models.Rola", b =>
                {
                    b.Property<int>("id_roli")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasColumnType("varchar(8)");

                    b.HasKey("id_roli");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            id_roli = 1,
                            nazwa = "admin"
                        },
                        new
                        {
                            id_roli = 2,
                            nazwa = "trener"
                        },
                        new
                        {
                            id_roli = 3,
                            nazwa = "dietetyk"
                        });
                });

            modelBuilder.Entity("WebApplication.Models.RolaUzytkownika", b =>
                {
                    b.Property<int>("id_roli")
                        .HasColumnType("int");

                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.HasKey("id_roli", "id_uzytkownika");

                    b.HasIndex("id_uzytkownika");

                    b.ToTable("RoleUzytkownikow");
                });

            modelBuilder.Entity("WebApplication.Models.Skladnik", b =>
                {
                    b.Property<int>("id_skladnika")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("bialko")
                        .HasColumnType("int");

                    b.Property<int>("id_kategorii")
                        .HasColumnType("int");

                    b.Property<int>("kalorie")
                        .HasColumnType("int");

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("tluszcze")
                        .HasColumnType("int");

                    b.Property<int>("waga")
                        .HasColumnType("int");

                    b.Property<int>("weglowodany")
                        .HasColumnType("int");

                    b.HasKey("id_skladnika");

                    b.HasIndex("id_kategorii");

                    b.ToTable("Skladniki");
                });

            modelBuilder.Entity("WebApplication.Models.Trening", b =>
                {
                    b.Property<int>("id_treningu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("id_kategorii")
                        .HasColumnType("int");

                    b.Property<int>("id_uzytkownika")
                        .HasColumnType("int");

                    b.Property<string>("nazwa")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.HasKey("id_treningu");

                    b.HasIndex("id_kategorii");

                    b.HasIndex("id_uzytkownika");

                    b.ToTable("Treningi");
                });

            modelBuilder.Entity("WebApplication.Models.TreningSzczegoly", b =>
                {
                    b.Property<int>("id_treningu")
                        .HasColumnType("int");

                    b.Property<int>("id_cwiczenia")
                        .HasColumnType("int");

                    b.Property<int>("liczba_powtorzen")
                        .HasColumnType("int");

                    b.HasKey("id_treningu", "id_cwiczenia");

                    b.HasIndex("id_cwiczenia");

                    b.ToTable("treningSzczegoly");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.Cwiczenie", b =>
                {
                    b.HasOne("WebApplication.Models.KategoriaCwiczenia", "kategoria")
                        .WithMany("cwiczenia")
                        .HasForeignKey("id_kategorii")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.HistoriaUzytkownika", b =>
                {
                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "uzytkownik")
                        .WithMany("historiaUzytkownika")
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.ObrazProfilowe", b =>
                {
                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "uzytkownik")
                        .WithOne("profilowe")
                        .HasForeignKey("WebApplication.Models.ObrazProfilowe", "id_uzytkownika")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.ObrazyPosilku", b =>
                {
                    b.HasOne("WebApplication.Models.Posilek", "posilek")
                        .WithMany("obrazy")
                        .HasForeignKey("id_posilku")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.ObrazyTreningu", b =>
                {
                    b.HasOne("WebApplication.Models.Trening", "trening")
                        .WithMany("obrazy")
                        .HasForeignKey("id_treningu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.Ocena", b =>
                {
                    b.HasOne("WebApplication.Models.Rola", "rola")
                        .WithMany()
                        .HasForeignKey("id_roli")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "oceniajacy")
                        .WithMany()
                        .HasForeignKey("id_uzytkownika_oceniajacego")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "oceniany")
                        .WithMany("oceny")
                        .HasForeignKey("id_uzytkownika_ocenianego")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.OcenaPosilku", b =>
                {
                    b.HasOne("WebApplication.Models.Posilek", "posilek")
                        .WithMany("oceny")
                        .HasForeignKey("id_posilku")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "oceniajacy")
                        .WithMany()
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.OcenaTreningu", b =>
                {
                    b.HasOne("WebApplication.Models.Trening", "trening")
                        .WithMany("oceny")
                        .HasForeignKey("id_treningu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "oceniajacy")
                        .WithMany()
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.PlanowaniePosilkow", b =>
                {
                    b.HasOne("WebApplication.Models.Posilek", "posilek")
                        .WithMany()
                        .HasForeignKey("id_posilku")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "uzytkownik")
                        .WithMany("planowanePosilki")
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.PlanowanieTreningow", b =>
                {
                    b.HasOne("WebApplication.Models.Trening", "trening")
                        .WithMany()
                        .HasForeignKey("id_treningu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "uzytkownik")
                        .WithMany("planowaneTreningi")
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.Posilek", b =>
                {
                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "uzytkownik")
                        .WithMany("posilki")
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.PosilekSzczegoly", b =>
                {
                    b.HasOne("WebApplication.Models.Posilek", "posilek")
                        .WithMany("skladniki")
                        .HasForeignKey("id_posilku")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Models.Skladnik", "skladnik")
                        .WithMany("posilki")
                        .HasForeignKey("id_skladnika")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.ProsbyOUprawnienia", b =>
                {
                    b.HasOne("WebApplication.Models.Rola", "rola")
                        .WithMany("prosby")
                        .HasForeignKey("id_roli")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "uzytkownik")
                        .WithMany("prosby")
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.RolaUzytkownika", b =>
                {
                    b.HasOne("WebApplication.Models.Rola", "rola")
                        .WithMany("uzytkownicy")
                        .HasForeignKey("id_roli")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "uzytkownik")
                        .WithMany("role")
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.Skladnik", b =>
                {
                    b.HasOne("WebApplication.Models.KategoriaSkladnikow", "kategoria")
                        .WithMany("skladniki")
                        .HasForeignKey("id_kategorii")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.Trening", b =>
                {
                    b.HasOne("WebApplication.Models.KategoriaTreningu", "kategoria")
                        .WithMany("treningi")
                        .HasForeignKey("id_kategorii")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Areas.Identity.Data.Uzytkownik", "uzytkownik")
                        .WithMany("treningi")
                        .HasForeignKey("id_uzytkownika")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication.Models.TreningSzczegoly", b =>
                {
                    b.HasOne("WebApplication.Models.Cwiczenie", "cwiczenie")
                        .WithMany("treningi")
                        .HasForeignKey("id_cwiczenia")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication.Models.Trening", "trening")
                        .WithMany("cwiczenia")
                        .HasForeignKey("id_treningu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
