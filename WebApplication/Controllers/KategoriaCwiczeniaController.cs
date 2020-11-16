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
using WebApplication.Areas.Identity.Data;
using System.Security.Claims;

namespace WebApplication.Controllers
{
    [Authorize]
    public class KategoriaCwiczeniaController : Controller
    {
        private readonly MyContext _context;

        public KategoriaCwiczeniaController(MyContext context)
        {
            _context = context;
        }

        // GET: KategoriaCwiczenia
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["currentSearchString"] = searchString;

            KategoriaCwiczenia defaultCategory = _context.kategoriaCwiczenia
                                                         .Where(k => k.nazwa == "inne")
                                                         .FirstOrDefault();
            ViewBag.DefaultCategory = defaultCategory;
            ViewBag.isTrainer = isTrainer();

            if (!String.IsNullOrEmpty(searchString))
            {
                return View(await _context.kategoriaCwiczenia.Where(k => k.nazwa.Contains(searchString)).ToListAsync());
            }
            else
            return View(await _context.kategoriaCwiczenia.ToListAsync());
        }

        // GET: KategoriaCwiczenia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoriaCwiczenia = await _context.kategoriaCwiczenia
                                                    .Include(k => k.cwiczenia)
                                                    .FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (kategoriaCwiczenia == null)
            {
                return NotFound();
            }

            return View(kategoriaCwiczenia);
        }

        // GET: KategoriaCwiczenia/Create
        public IActionResult Create()
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            return View();
        }

        // POST: KategoriaCwiczenia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_kategorii,nazwa")] KategoriaCwiczenia kategoriaCwiczenia)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                _context.Add(kategoriaCwiczenia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategoriaCwiczenia);
        }

        // GET: KategoriaCwiczenia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            var kategoriaCwiczenia = await _context.kategoriaCwiczenia.FindAsync(id);
            if (kategoriaCwiczenia == null)
            {
                return NotFound();
            }
            return View(kategoriaCwiczenia);
        }

        // POST: KategoriaCwiczenia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_kategorii,nazwa")] KategoriaCwiczenia kategoriaCwiczenia)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (id != kategoriaCwiczenia.id_kategorii)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategoriaCwiczenia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriaCwiczeniaExists(kategoriaCwiczenia.id_kategorii))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kategoriaCwiczenia);
        }

        // GET: KategoriaCwiczenia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            var kategoriaCwiczenia = await _context.kategoriaCwiczenia
                .FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (kategoriaCwiczenia == null)
            {
                return NotFound();
            }

            return View(kategoriaCwiczenia);
        }

        // POST: KategoriaCwiczenia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            KategoriaCwiczenia defaultCategory = _context.kategoriaCwiczenia
                                                         .Where(k => k.nazwa == "inne")
                                                         .FirstOrDefault();
            if(defaultCategory.id_kategorii == id)
                return RedirectToAction("Index");

            List<Cwiczenie> cwiczenia = _context.cwiczenia.Where(k => k.id_kategorii == id).ToList();

            foreach (Cwiczenie cw in cwiczenia)
                cw.id_kategorii = defaultCategory.id_kategorii;

            _context.SaveChanges();

            var kategoriaCwiczenia = await _context.kategoriaCwiczenia.FindAsync(id);
            _context.kategoriaCwiczenia.Remove(kategoriaCwiczenia);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool KategoriaCwiczeniaExists(int id)
        {
            return _context.kategoriaCwiczenia.Any(e => e.id_kategorii == id);
        }

        private bool isTrainer()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            List<RolaUzytkownika> usersRoles = _context.RolaUzytkownika.Where(k => k.id_uzytkownika == userId).Include(c => c.rola).ToList();

            foreach (var usersRole in usersRoles)
                if (usersRole.rola.nazwa == "trener" || usersRole.rola.nazwa == "admin")
                    return true;
            return false;
        }
    }
}
