using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using WebApplication.Data;

using Microsoft.AspNetCore.Mvc.Rendering;



namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MyContext _context;


        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;

            //_context.Add(new HistoriaUzytkownika());
            //hahahahahah
        }

        public IActionResult Index(DateTime sellected_date)
        {
            DateTime first = new DateTime(01,01,0001);
            if (sellected_date == first)
            {
                sellected_date = DateTime.Today;
            }
            if (User.Identity.IsAuthenticated)
            {
                double bmi = 0;
                string name = User.Identity.Name;
                var user = _context.uzytkownicy.Single(e => e.login == name);

                if (_context.historiaUzytkownika.Any(e => e.data.Date == sellected_date))
                {
                }
                else if(!_context.historiaUzytkownika.Any(e => e.data.Date == sellected_date) && _context.historiaUzytkownika.Any())
                {
                    var dayBefore = _context.historiaUzytkownika.Single(e => e.data == _context.historiaUzytkownika.Select(e => e.data).Max());
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
                bmi = now.waga / ((now.wzrost/100) ^ 2);
                ViewBag.user = user;
                ViewBag.bmi = bmi;
                ViewBag.selected = sellected_date;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
