using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.Data;



namespace WebApplication.Controllers
{
    public class UserHubController : Controller
    {


        private readonly MyContext _context;

        public UserHubController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime sellected_date)
        {

            DateTime first = new DateTime(01, 01, 0001);
            if (sellected_date == first)
            {

                sellected_date = DateTime.Today;
            }
            if (User.Identity.IsAuthenticated)
            {
                double bmi = 0;
                int userid = int.Parse(User.Identity.GetUserId());
                var user = _context.uzytkownicy.Single(e => e.Id == userid);
                user.limit = 2000;


                if (_context.historiaUzytkownika.Any(e => e.data.Date == sellected_date && e.id_uzytkownika == user.Id))
                {
                }
                else if (!_context.historiaUzytkownika.Any(e => e.data.Date == sellected_date && e.id_uzytkownika == user.Id) && _context.historiaUzytkownika.Any(e => e.id_uzytkownika == user.Id))
                {
                    var warunek = _context.historiaUzytkownika.Where(e => e.id_uzytkownika == user.Id).ToList();
                    var dayBefore = warunek.Single(e => e.data == warunek.Select(e => e.data).Max());
                    HistoriaUzytkownika hs = new HistoriaUzytkownika();
                    hs.id_uzytkownika = user.Id;
                    hs.data = sellected_date;
                    hs.waga = dayBefore.waga;
                    hs.wzrost = dayBefore.wzrost;
                    hs.uzytkownik = user;
                    _context.Add(hs);
                    _context.SaveChanges();

                }
                else
                {
                    HistoriaUzytkownika hs = new HistoriaUzytkownika();
                    hs.id_uzytkownika = user.Id;
                    hs.data = sellected_date;
                    hs.waga = 0;
                    hs.wzrost = 0;
                    hs.uzytkownik = user;
                    _context.Add(hs);
                    _context.SaveChanges();
                }
                var his = _context.historiaUzytkownika.Where(e => e.id_uzytkownika == user.Id && e.data.Date == sellected_date).ToList();
                var now = his.Single(e => e.data == his.Select(e => e.data).Max());
                bmi = now.waga / ((now.wzrost / 100) ^ 2);

                var posilki = _context.planowanePosilki.Include(e => e.posilek)
                    .Where(e=> e.id_uzytkownika == user.Id && e.data.Day == sellected_date.Day).ToList();
                foreach (var item in posilki)
                {
                    user.limit -= item.posilek.kalorie;
                }
                ViewBag.user = user;
                ViewBag.bmi = bmi;
                ViewBag.selected = sellected_date;
                ViewBag.posilki = posilki;
            }
            return View();
        }

        public IActionResult Admin()
        {
            bool isadmin = isAdmin();
            ViewBag.isadmin = isadmin;
            var cwiczenie = _context.cwiczenia.ToList();
            var trening = _context.treningi.ToList();
            var kat_treningu = _context.kategoriaTreningu.ToList();
            var kat_cwiczenia = _context.kategoriaCwiczenia.ToList();
            var skladnik = _context.skladnik.ToList();
            var posilek = _context.posilki.ToList();
            var kat_skladnika = _context.kategoriaSkladnikow.ToList();
            var role = _context.RolaUzytkownika.Include(e=>e.rola).ToList();
            var planowanie_trening = _context.planowaneTreningi.ToList();
            ViewBag.cwiczenie = cwiczenie;
            ViewBag.trening = trening;
            ViewBag.kat_treningu = kat_treningu;
            ViewBag.kat_cwiczenia = kat_cwiczenia;
            ViewBag.skladnik = skladnik;
            ViewBag.posilek = posilek;
            ViewBag.kat_skladnika = kat_skladnika;
            ViewBag.role = role;
            ViewBag.planowanie_trening = planowanie_trening;
            return View();
        }

        private bool isAdmin()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.Identity.GetUserId());
                List<RolaUzytkownika> usersRoles = _context.RolaUzytkownika.Where(k => k.id_uzytkownika == userId).Include(c => c.rola).ToList();
                foreach (var usersRole in usersRoles)
                    if (usersRole.rola.nazwa == "admin")
                    {
                        return true;
                    }
            }
            
            return false;
        }
    }
}

