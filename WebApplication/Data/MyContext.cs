using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication.Data
{
    public class MyContext: IdentityDbContext<Uzytkownik, IdentityRole<int>, int>
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
         
            //mapowanie kluczy głównych i obcych
            builder.Entity<Uzytkownik>(b =>
            {
                b.HasMany<RolaUzytkownika>(t => t.role)
                 .WithOne(t => t.uzytkownik)
                 .HasForeignKey(uc => uc.id_uzytkownika);

                b.HasMany<HistoriaUzytkownika>(t => t.historiaUzytkownika)
                 .WithOne(t => t.uzytkownik)
                 .HasForeignKey(uc => uc.id_uzytkownika);

                b.HasMany<Trening>(t => t.treningi)
                 .WithOne(t => t.uzytkownik)
                 .HasForeignKey(uc => uc.id_uzytkownika);

                b.HasMany<Posilek>(t => t.posilki)
                 .WithOne(t => t.uzytkownik)
                 .HasForeignKey(uc => uc.id_uzytkownika);
            });

            builder.Entity<PosilekSzczegoly>(b =>
            {
                b.HasKey(t => new { t.id_posilku, t.id_skladnika });
            });

            builder.Entity<TreningSzczegoly>(b =>
            {
                b.HasKey(t => new { t.id_treningu, t.id_cwiczenia });
            });

            builder.Entity<RolaUzytkownika>(b =>
            {
                b.HasKey(t => new { t.id_uzytkownika, t.id_roli });
            });


            builder.Entity<Trening>().HasKey(t => t.id_treningu);
            builder.Entity<Trening>()
                .HasOne<Uzytkownik>(t => t.uzytkownik)
                .WithMany(t => t.treningi)
                .HasForeignKey(d => d.id_uzytkownika);

            builder.Entity<Trening>()
                .HasOne<KategoriaTreningu>(t => t.kategoria)
                .WithMany(t => t.treningi)
                .HasForeignKey(d => d.id_kategorii);

            builder.Entity<Rola>().HasKey(t => t.id_roli);

            builder.Entity<Posilek>().HasKey(t => t.id_posilku);
            builder.Entity<Posilek>()
                .HasOne<Uzytkownik>(t => t.uzytkownik)
                .WithMany(t => t.posilki)
                .HasForeignKey(d => d.id_uzytkownika);

            builder.Entity<KategoriaSkladnikow>().HasKey(t => t.id_kategorii);
            builder.Entity<KategoriaTreningu>().HasKey(t => t.id_kategorii);
            builder.Entity<KategoriaCwiczenia>().HasKey(t => t.id_kategorii);

            builder.Entity<Cwiczenie>().HasKey(t => t.id_cwiczenia);
            builder.Entity<Cwiczenie>()
                .HasOne<KategoriaCwiczenia>(t => t.kategoria)
                .WithMany(t => t.cwiczenia)
                .HasForeignKey(d => d.id_kategorii);

            builder.Entity<Skladnik>().HasKey(t => t.id_skladnika);
            builder.Entity<Skladnik>()
                .HasOne<KategoriaSkladnikow>(t => t.kategoria)
                .WithMany(t => t.skladniki)
                .HasForeignKey(d => d.id_kategorii);

            //builder.Entity<HistoriaUzytkownika>().HasKey(t => t.id_historia);
            builder.Entity<HistoriaUzytkownika>().HasKey(t => new { t.id_uzytkownika, t.data });
            builder.Entity<HistoriaUzytkownika>()
                .HasOne<Uzytkownik>(t => t.uzytkownik)
                .WithMany(t => t.historiaUzytkownika)
                .HasForeignKey(d => d.id_uzytkownika)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TreningSzczegoly>().HasKey(t => new { t.id_treningu, t.id_cwiczenia });
            builder.Entity<TreningSzczegoly>()
                .HasOne<Trening>(t => t.trening)
                .WithMany(t => t.cwiczenia)
                .HasForeignKey(d => d.id_treningu)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TreningSzczegoly>()
                .HasOne<Cwiczenie>(t => t.cwiczenie)
                .WithMany(t => t.treningi)
                .HasForeignKey(d => d.id_cwiczenia)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PosilekSzczegoly>().HasKey(t => new { t.id_posilku, t.id_skladnika });
            builder.Entity<PosilekSzczegoly>()
                .HasOne<Posilek>(t => t.posilek)
                .WithMany(t => t.skladniki)
                .HasForeignKey(d => d.id_posilku)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PosilekSzczegoly>()
                .HasOne<Skladnik>(t => t.skladnik)
                .WithMany(t => t.posilki)
                .HasForeignKey(d => d.id_skladnika)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RolaUzytkownika>().HasKey(t => new { t.id_roli, t.id_uzytkownika });
            builder.Entity<RolaUzytkownika>()
                .HasOne<Rola>(t => t.rola)
                .WithMany(t => t.uzytkownicy)
                .HasForeignKey(d => d.id_roli)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RolaUzytkownika>()
                .HasOne<Uzytkownik>(t => t.uzytkownik)
                .WithMany(t => t.role)
                .HasForeignKey(d => d.id_uzytkownika)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<PlanowaniePosilkow>().HasKey(t => t.id);
            builder.Entity<PlanowaniePosilkow>()
                .HasOne<Uzytkownik>(t => t.uzytkownik)
                .WithMany(t => t.planowanePosilki)
                .HasForeignKey(d => d.id_uzytkownika)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PlanowaniePosilkow>()
                .HasOne<Posilek>(t => t.posilek)
                .WithMany()
                .HasForeignKey(d => d.id_posilku)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<PlanowanieTreningow>().HasKey(t => t.id );
            builder.Entity<PlanowanieTreningow>()
                .HasOne<Uzytkownik>(t => t.uzytkownik)
                .WithMany(t => t.planowaneTreningi)
                .HasForeignKey(d => d.id_uzytkownika)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PlanowanieTreningow>()
                .HasOne<Trening>(t => t.trening)
                .WithMany()
                .HasForeignKey(d => d.id_treningu)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Ocena>().HasKey(t => new { t.id_uzytkownika_ocenianego, t.id_uzytkownika_oceniajacego, t.id_roli });
            builder.Entity<Ocena>()
                .HasOne<Rola>(t => t.rola)
                .WithMany()
                .HasForeignKey(d => d.id_roli)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Ocena>()
                .HasOne<Uzytkownik>(t => t.oceniany)
                .WithMany(t => t.oceny)
                .HasForeignKey(d => d.id_uzytkownika_ocenianego)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Ocena>()
                .HasOne<Uzytkownik>(t => t.oceniajacy)
                .WithMany()
                .HasForeignKey(d => d.id_uzytkownika_oceniajacego)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<KategoriaCwiczenia>().HasData(
                                new { id_kategorii = 1, nazwa = "inne" });

            builder.Entity<KategoriaTreningu>().HasData(
                                new { id_kategorii = 1, nazwa = "inne" });

            builder.Entity<KategoriaSkladnikow>().HasData(
                                new { id_kategorii = 1, nazwa = "inne" });

            builder.Entity<Rola>().HasData(
                                new { id_roli = 1, nazwa = "admin" });

            builder.Entity<Rola>().HasData(
                                new { id_roli = 2, nazwa = "trener" });

            builder.Entity<Rola>().HasData(
                                new { id_roli = 3, nazwa = "dietetyk" });
        }

        public DbSet<Cwiczenie> cwiczenia { get; set; }
        public DbSet<Trening> treningi { get; set; }
        public DbSet<KategoriaTreningu> kategoriaTreningu { get; set; }
        public DbSet<TreningSzczegoly> treningSzczegoly { get; set; }
        public DbSet<Posilek> posilki { get; set; }
        public DbSet<Skladnik> skladnik { get; set; }
        public DbSet<PosilekSzczegoly> posilekSzczegoly { get; set; }
        public DbSet<KategoriaCwiczenia> kategoriaCwiczenia { get; set; }
        public DbSet<Uzytkownik> uzytkownicy { get; set; }
        public DbSet<HistoriaUzytkownika> historiaUzytkownika { get; set; }
        public DbSet<Rola> role { get; set; }
        public DbSet<KategoriaSkladnikow> kategoriaSkladnikow { get; set; }
        public DbSet<RolaUzytkownika> RolaUzytkownika { get; set; }
        public DbSet<PlanowaniePosilkow> planowanePosilki { get; set; }
        public DbSet<PlanowanieTreningow> planowaneTreningi { get; set; }
        public DbSet<Ocena> oceny { get; set; }
        public IEnumerable<object> Rola { get; internal set; }
    }
}
