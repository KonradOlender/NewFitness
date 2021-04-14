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
    public class KategoriaSkladnikowController : Controller
    {
        private readonly MyContext _context;

        public KategoriaSkladnikowController(MyContext context)
        {
            _context = context;
        }

        // GET: KategoriaSkladnikow
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["currentSearchString"] = searchString;
            KategoriaSkladnikow defaultCategory = _context.kategoriaSkladnikow.Where(k => k.nazwa == "inne").FirstOrDefault();
            ViewBag.DeraultCategory = defaultCategory;
            ViewBag.isDietician = isDietician();

            if (!String.IsNullOrEmpty(searchString))
            {
                return View(await _context.kategoriaSkladnikow.Where(k => k.nazwa.Contains(searchString)).ToListAsync());
            }
            else
                return View(await _context.kategoriaSkladnikow.ToListAsync());
        }

        // GET: KategoriaSkladnikow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.kategoriaSkladnikow
                .FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (category == null)
            {
                return NotFound();
            }

            var category_meals = await _context.kategoriaSkladnikow.Include(k => k.skladniki)
                .FirstOrDefaultAsync(m => m.id_kategorii == id);

            ViewBag.meals = category_meals.skladniki;

            return View(category);
        }

        // GET: KategoriaSkladnikow/Create
        public IActionResult Create()
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            return View();
        }

        // POST: KategoriaSkladnikow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_kategorii,nazwa")] KategoriaSkladnikow category)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: KategoriaSkladnikow/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.kategoriaSkladnikow.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: KategoriaSkladnikow/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_kategorii,nazwa")] KategoriaSkladnikow category)
        {
            if (!this.isDietician())
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
                    if (!KategoriaSkladnikowExists(category.id_kategorii))
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

        // GET: KategoriaSkladnikow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.kategoriaSkladnikow
                .FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: KategoriaSkladnikow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            KategoriaSkladnikow defaultCategory = _context.kategoriaSkladnikow.Where(k => k.nazwa == "inne").FirstOrDefault();

            if (defaultCategory.id_kategorii == id)
                return RedirectToAction("Index");

            List<Skladnik> skladniki = _context.skladnik.Where(k => k.id_kategorii == id).ToList();

            foreach (Skladnik sk in skladniki)
                sk.id_kategorii = defaultCategory.id_kategorii;

            _context.SaveChanges();

            var category = await _context.kategoriaSkladnikow.FindAsync(id);
            _context.kategoriaSkladnikow.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategoriaSkladnikowExists(int id)
        {
            return _context.kategoriaSkladnikow.Any(e => e.id_kategorii == id);
        }

        private bool isDietician()
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
                if (usersRole.rola.nazwa == "dietetyk")
                    return true;
            }
            return false;
        }
    }
}
