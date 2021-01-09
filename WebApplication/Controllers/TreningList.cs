using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.Data;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Lista(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.treningi
                .Include(t => t.kategoria)
                .Include(t => t.uzytkownik)
                .FirstOrDefaultAsync(m => m.id_treningu == id);

            ViewBag.trainingDetails = _context.treningSzczegoly.Where(k => k.id_treningu == id)
                                        .Include(k => k.cwiczenie)
                                        .ToList();

            ViewBag.userId = int.Parse(this.User.Identity.GetUserId());
            ViewBag.treningOwner = trening.id_uzytkownika;
            ViewBag.index = id;

            if (trening == null)
            {
                return NotFound();
            }

            return View(trening);
        }

    }
}
