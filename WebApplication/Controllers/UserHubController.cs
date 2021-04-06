using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.Data;
using Microsoft.AspNetCore.Hosting;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers
{
    public class UserHubController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserHubController(IWebHostEnvironment hostingEnvironment, MyContext context )
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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

                int bialko = 0;
                int tluszcz = 0;
                int weng = 0;


                if (_context.historiaUzytkownika.Any(e => e.data.Date == sellected_date && e.id_uzytkownika == user.Id))
                {
                }
                else 
                if (!_context.historiaUzytkownika.Any(
                            e => e.data.Date == sellected_date && e.id_uzytkownika == user.Id) && _context.historiaUzytkownika.Any(e => e.id_uzytkownika == user.Id))
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
                    bialko += getBialko(item.id_posilku);
                    tluszcz += getTluszcze(item.id_posilku);
                    weng += getWeglowodany(item.id_posilku);
                }

                List<HistoriaUzytkownika> hist = _context.historiaUzytkownika.Where(e => e.id_uzytkownika == userid && e.data.Month == sellected_date.Month).ToList();
                List<float> postep = new List<float>();
                List<int> dates = new List<int>();
                foreach (var item in hist)
                {
                    postep.Add((float)item.waga);
                    dates.Add(item.data.Day);
                }
                ViewBag.postep = postep.ToList();
                ViewBag.dates = dates.ToList();
                ViewBag.postepCount = postep.Count;
                ViewBag.user = user;
                ViewBag.bmi = bmi;
                ViewBag.selected = sellected_date;
                ViewBag.posilki = posilki;

                ViewBag.bialko = bialko;
                ViewBag.tluszcz = tluszcz;
                ViewBag.weng = weng;
            }
            return View();
        }

        public IActionResult Admin()
        {
            bool isadmin = isAdmin();
            if(!isadmin)
            {
                return RedirectToAction("Index");
            }

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
            var oceny = _context.oceny.ToList();
            var ocenyTrening = _context.ocenyTreningow.ToList();
            var messages = _context.messages.ToList();
            var chats = _context.chats.ToList();
            ViewBag.cwiczenie = cwiczenie;
            ViewBag.trening = trening;
            ViewBag.kat_treningu = kat_treningu;
            ViewBag.kat_cwiczenia = kat_cwiczenia;
            ViewBag.skladnik = skladnik;
            ViewBag.posilek = posilek;
            ViewBag.kat_skladnika = kat_skladnika;
            ViewBag.role = role;
            ViewBag.planowanie_trening = planowanie_trening;
            ViewBag.oceny = oceny;
            ViewBag.ocenyTrening = ocenyTrening;
            ViewBag.messages = messages;
            ViewBag.chats = chats;
            return View();
        }

        public IActionResult Kalkulator()
        {
            return View();
        }

        //Do dodania (jeślu bez daty to -500 kcal)
        [HttpPost]
        public async Task<IActionResult> Kalkulator(int waga, int goal, int? wzrost, int? plec, int? aktywnosc, DateTime data)
        {
            int userid = int.Parse(User.Identity.GetUserId());
            var user = _context.uzytkownicy.Single(e => e.Id == userid);
            int kcal;
            DateTime data1 = DateTime.Today;
            user.cel = goal;
            double weeks = (data - data1).TotalDays / 7;
            double roznica = goal - waga;
            double wynik;
            if (plec == 1)
            {
                if (aktywnosc==1)
                {
                    kcal = 2600;
                }
                else if(aktywnosc == 2)
                {
                    kcal = 2900;
                }
                else
                {
                    kcal = 3100;
                }
            }
            else
            {
                if (aktywnosc == 1)
                {
                    kcal = 2000;
                }
                else if (aktywnosc == 2)
                {
                    kcal = 2200;
                }
                else
                {
                    kcal = 2500;
                }
            }
            wynik = kcal + 1000*(roznica/weeks);
            ViewBag.userId = userid;
            ViewBag.wynik = wynik;
            user.limit = (int)wynik;
            _context.uzytkownicy.Update(user);
            await _context.SaveChangesAsync();

            return View();
        }
    
        //czemu url jest zahardcodowany ?
        [Authorize]
        public IActionResult ExportToPDF()
        {
            //Set Environment variable for OpenSSL assemblies folder 
            string SSLPath = Path.Combine(_hostingEnvironment.ContentRootPath, "OpenSSL");
            Environment.SetEnvironmentVariable("Path", SSLPath);

            //Initialize HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            WebKitConverterSettings settings = new WebKitConverterSettings();
            //Set WebKit path
            settings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "QtBinariesWindows");
            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //Convert URL to PDF
            int id = int.Parse(User.Identity.GetUserId());
            string url = "https://localhost:44327/UserHub/Pdf/" + id;
            PdfDocument document = htmlConverter.Convert(url);

            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            DateTime today = DateTime.Today;
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Raport-" + today.ToShortDateString() + ".pdf");
        }

        public IActionResult Pdf(int? id)
        {
            if (id != null)
            {
                if (User.Identity.IsAuthenticated)
                    if (id != int.Parse(User.Identity.GetUserId()))
                        return NotFound();

                var user = _context.uzytkownicy.Single(e => e.Id == id);
                ViewBag.user = user;
                DateTime today = DateTime.Today;
                ViewBag.date = today;

                var usersProfile = _context.uzytkownicy.Where(k => k.Id == id)
                                            .Include(k => k.profilowe);
                try
                {
                    if (usersProfile.First().profilowe == null)
                        ViewBag.image = null;
                    else
                        ViewBag.image = usersProfile.First().profilowe.GetImageDataUrl();
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index");
                }

                List<RolaUzytkownika> usersRoles = _context.RolaUzytkownika.Where(k => k.id_uzytkownika == id).Include(c => c.rola).ToList();
                String roles = "";
                foreach (var usersRole in usersRoles)
                {
                    roles += usersRole.rola.nazwa + " ";
                }
                ViewBag.roles = roles;

                var his = _context.historiaUzytkownika.Where(e => e.id_uzytkownika == user.Id && e.data.Date == today).ToList();
                try
                {
                    var now = his.Single(e => e.data == his.Select(e => e.data).Max());
                    double bmi = now.waga / ((now.wzrost / 100) ^ 2);
                    ViewBag.now = now;
                    ViewBag.bmi = bmi;  
                }
                catch
                {
                    return NotFound();
                }

                var posilki = _context.planowanePosilki.Include(e => e.posilek)
                    .Where(e => e.id_uzytkownika == user.Id && e.data.Day == today.Day && e.data.Month == today.Month && e.data.Year == today.Year)
                    .ToList();
                ViewBag.posilki = posilki;

                int sum = 0;
                foreach (var p in posilki) sum += p.posilek.kalorie;
                ViewBag.sumKal = sum;

                return View();
            }
            return NotFound();

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

        public int getWeglowodany(int meal_id)
        {
            List<PosilekSzczegoly> meals = _context.posilekSzczegoly.Include(t => t.skladnik).Where(t => t.id_posilku == meal_id).ToList();
            int sum = 0;
            foreach (var ps in meals)
            {
                sum += ps.skladnik.weglowodany * ps.porcja / 100;
            }
            return sum;
        }

        public int getBialko(int meal_id)
        {
            List<PosilekSzczegoly> meals = _context.posilekSzczegoly.Include(t => t.skladnik).Where(t => t.id_posilku == meal_id).ToList();
            int sum = 0;
            foreach (var ps in meals)
            {
                sum += ps.skladnik.bialko * ps.porcja / 100;
            }
            return sum;
        }

        public int getTluszcze(int meal_id)
        {
            List<PosilekSzczegoly> meals = _context.posilekSzczegoly.Include(t => t.skladnik).Where(t => t.id_posilku == meal_id).ToList();
            int sum = 0;
            foreach (var ps in meals)
            {
                sum += ps.skladnik.tluszcze * ps.porcja / 100;
            }
            return sum;
        }
    }
}

