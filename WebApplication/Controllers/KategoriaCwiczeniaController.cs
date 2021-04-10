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

            var category = await _context.kategoriaCwiczenia
                                                    .Include(k => k.cwiczenia)
                                                    .FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (category == null)
            {
                return NotFound();
            }
            this.isTrainer();
            return View(category);
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
        public async Task<IActionResult> Create([Bind("id_kategorii,nazwa")] KategoriaCwiczenia category)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
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

            var category = await _context.kategoriaCwiczenia.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: KategoriaCwiczenia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_kategorii,nazwa")] KategoriaCwiczenia category)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (id != category.id_kategorii)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriaCwiczeniaExists(category.id_kategorii))
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
            return View(category);
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

            var category = await _context.kategoriaCwiczenia
                .FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
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

            List<Cwiczenie> exercises = _context.cwiczenia.Where(k => k.id_kategorii == id).ToList();

            foreach (Cwiczenie cw in exercises)
                cw.id_kategorii = defaultCategory.id_kategorii;

            _context.SaveChanges();

            var category = await _context.kategoriaCwiczenia.FindAsync(id);
            _context.kategoriaCwiczenia.Remove(category);
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
            {
                if (usersRole.rola.nazwa == "admin")
                {
                    ViewBag.ifAdmin = true;
                    return true;
                }
                if (usersRole.rola.nazwa == "trener")
                    return true;
            }
            return false;
        }
    }
}
