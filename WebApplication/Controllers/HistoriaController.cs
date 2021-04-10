using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    public class HistoriaController : Controller
    {
        private readonly MyContext _context;

        public HistoriaController(MyContext context)
        {
            _context = context;
        }

        // GET: Historia
        public async Task<IActionResult> Index()
        {
            int users_id = int.Parse(User.Identity.GetUserId());
            var myContext = _context.historiaUzytkownika.Where(k => k.id_uzytkownika==users_id);
            this.isAdmin();
            return View(await myContext.ToListAsync());
        }

        public async Task<IActionResult> Done()
        {
            this.isAdmin();
            return View();
        }

        // GET: Historia/Create
        public IActionResult Create()
        {
            this.isAdmin();
            return View();
        }

        // POST: Historia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_historia,data,waga,wzrost")] HistoriaUzytkownika usersHistory)
        {
            int users_id = int.Parse(User.Identity.GetUserId());
            if (this.HistoriaUzytkownikaExists(users_id, usersHistory.data))
                return View("AlreadyExists");                                                              

            if (ModelState.IsValid)
            {
                usersHistory.id_uzytkownika = users_id;
                _context.Add(usersHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Done));
            }
            this.isAdmin();
            return View(usersHistory);
        }

        // GET: Historia/Delete/5
        public async Task<IActionResult> Delete(String date)
        {
            if (date == null)
            {
                return NotFound();
            }
            int users_id = int.Parse(User.Identity.GetUserId());
            var usersHistory = await _context.historiaUzytkownika
                .FirstOrDefaultAsync(m => m.id_uzytkownika == users_id && m.data == Convert.ToDateTime(date));
            if (usersHistory == null)
            {
                return View("UnableToDelete");
            }
            ViewBag.date = usersHistory.data;
            this.isAdmin();
            return View(usersHistory);
        }

        // POST: Historia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(String date)
        {
            int users_id = int.Parse(User.Identity.GetUserId());
            var usersHistory = await _context.historiaUzytkownika
                .FirstOrDefaultAsync(m => m.id_uzytkownika == users_id && m.data == Convert.ToDateTime(date));
            if (usersHistory == null)
            {
                return View("UnableToDelete");
            }
            _context.historiaUzytkownika.Remove(usersHistory);
            await _context.SaveChangesAsync();
            this.isAdmin();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoriaUzytkownikaExists(int id)
        {
            return _context.historiaUzytkownika.Any(e => e.id_uzytkownika == id);
        }

        private bool HistoriaUzytkownikaExists(int id, DateTime date)
        {
            return _context.historiaUzytkownika.Any(e => e.id_uzytkownika == id && e.data == date);
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
                        ViewBag.ifAdmin = true;
                        return true;
                    }
            }
            return false;
        }
    }
}
