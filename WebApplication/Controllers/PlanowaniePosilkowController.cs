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
    public class PlanowaniePosilkowController : Controller
    {
        private readonly MyContext _context;

        public PlanowaniePosilkowController(MyContext context)
        {
            _context = context;
        }

        // GET: PlanowaniePosilkow
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            var myContext = _context.planowanePosilki.Include(p => p.posilek)
                                                     .Where(x => x.id_uzytkownika == userId);
            return View(await myContext.ToListAsync());
        }

        // GET: PlanowaniePosilkow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userId = int.Parse(User.Identity.GetUserId());
            var planowaniePosilkow = await _context.planowanePosilki
                .Include(p => p.posilek)
                .Include(p => p.uzytkownik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (planowaniePosilkow == null)
            {
                return NotFound();
            }

            if (planowaniePosilkow.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            return View(planowaniePosilkow);
        }


        // GET: PlanowaniePosilkow/Create
        public IActionResult Create(string id_string)
        {
            ViewData["id_posilku"] = new SelectList(_context.posilki, "id_posilku", "nazwa");
            if (!String.IsNullOrEmpty(id_string))
            {
                int id = int.Parse(id_string);
                PlanowaniePosilkow meal = new PlanowaniePosilkow();
                meal.id_posilku = id;
                meal.data = DateTime.Now;
                return View(meal);
            }

            return View();
        }

        // POST: PlanowaniePosilkow/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_uzytkownika,id_posilku,data")] PlanowaniePosilkow planowaniePosilkow)
        {
            if (ModelState.IsValid)
            {
                planowaniePosilkow.id_uzytkownika = int.Parse(User.Identity.GetUserId());
                _context.Add(planowaniePosilkow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_posilku"] = new SelectList(_context.posilki, "id_posilku", "nazwa", planowaniePosilkow.id_posilku);
            return View(planowaniePosilkow);
        }

        // GET: PlanowaniePosilkow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planowaniePosilkow = await _context.planowanePosilki
                .Include(p => p.posilek)
                .Include(p => p.uzytkownik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (planowaniePosilkow == null)
            {
                return NotFound();
            }

            if (planowaniePosilkow.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            return View(planowaniePosilkow);
        }

        // POST: PlanowaniePosilkow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planowaniePosilkow = await _context.planowanePosilki.FindAsync(id);

            if (planowaniePosilkow.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            _context.planowanePosilki.Remove(planowaniePosilkow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanowaniePosilkowExists(int id)
        {
            return _context.planowanePosilki.Any(e => e.id_posilku == id);
        }
    }
}
