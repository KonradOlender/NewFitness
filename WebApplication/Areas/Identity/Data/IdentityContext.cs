using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication.Areas.Identity.Data;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class IdentityContext<TUser>
    : IdentityUserContext<TUser, int>
        where TUser : Uzytkownik
    {
        public IdentityContext(DbContextOptions<IdentityContext<TUser>> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            /*builder.Entity<Uzytkownik>(b =>
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
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}
