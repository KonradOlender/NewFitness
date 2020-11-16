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

            var kategoriaSkladnikow = await _context.kategoriaSkladnikow
                .FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (kategoriaSkladnikow == null)
            {
                return NotFound();
            }

            return View(kategoriaSkladnikow);
        }

        // GET: KategoriaSkladnikow/Create
        public IActionResult Create()
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            return View();
        }

        // POST: KategoriaSkladnikow/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_kategorii,nazwa")] KategoriaSkladnikow kategoriaSkladnikow)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                _context.Add(kategoriaSkladnikow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategoriaSkladnikow);
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

            var kategoriaSkladnikow = await _context.kategoriaSkladnikow.FindAsync(id);
            if (kategoriaSkladnikow == null)
            {
                return NotFound();
            }
            return View(kategoriaSkladnikow);
        }

        // POST: KategoriaSkladnikow/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_kategorii,nazwa")] KategoriaSkladnikow kategoriaSkladnikow)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            if (id != kategoriaSkladnikow.id_kategorii)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategoriaSkladnikow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriaSkladnikowExists(kategoriaSkladnikow.id_kategorii))
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
            return View(kategoriaSkladnikow);
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

            var kategoriaSkladnikow = await _context.kategoriaSkladnikow
                .FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (kategoriaSkladnikow == null)
            {
                return NotFound();
            }

            return View(kategoriaSkladnikow);
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

            var kategoriaSkladnikow = await _context.kategoriaSkladnikow.FindAsync(id);
            _context.kategoriaSkladnikow.Remove(kategoriaSkladnikow);
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
                if (usersRole.rola.nazwa == "dietetyk" || usersRole.rola.nazwa == "admin")
                    return true;
            return false;
        }
    }
}
