using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using Microsoft.AspNet.Identity;
using System.Data.Entity;


namespace WebApplication.Controllers
{
    [Authorize]
    public class TreningList : Controller
    {
        private readonly MyContext _context;

        public TreningList(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.Identity.GetUserId());
                var listaplantrening = _context.planowaneTreningi.Where(e => e.id_uzytkownika == userId);
                var listatrening = _context.treningi.ToList();
                var poniedzialki = listaplantrening.Where(e => e.dzien == "Poniedziałek").ToList();
                var wtorki = listaplantrening.Where(e => e.dzien == "Wtorek").ToList();
                var sroda = listaplantrening.Where(e => e.dzien == "Środa").ToList();
                var czwartek = listaplantrening.Where(e => e.dzien == "Czwartek").ToList();
                var piatek = listaplantrening.Where(e => e.dzien == "Piątek").ToList();
                var sobota = listaplantrening.Where(e => e.dzien == "Sobota").ToList();
                var niedziela = listaplantrening.Where(e => e.dzien == "Niedziela").ToList();
                int i = poniedzialki.Count;
                ViewBag.listaplantrening = listaplantrening;
                ViewBag.listatrening = listatrening;
                ViewBag.poniedzialki = poniedzialki;
                ViewBag.wtorki = wtorki;
                ViewBag.sroda = sroda;
                ViewBag.czwartek = czwartek;
                ViewBag.piatek = piatek;
                ViewBag.sobota = sobota;
                ViewBag.niedziela = niedziela;
            }

            return View();
        }
        //nie działa
        public IActionResult Lista(int index)
        {
            var trening = _context.treningSzczegoly.Where(k => k.id_treningu == index)
                                        .Include(k => k.cwiczenie)
                                        .ToList(); ;
            ViewBag.trening = trening;

            return View();
        }

    }
}
