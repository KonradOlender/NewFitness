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
    public class KategoriaTreninguController : Controller
    {
        private readonly MyContext _context;

        public KategoriaTreninguController(MyContext context)
        {
            _context = context;
        }

        // GET: KategoriaTreningu
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["currentSearchString"] = searchString;
            KategoriaTreningu defaultCategory = _context.kategoriaTreningu
                                                         .Where(k => k.nazwa == "inne")
                                                         .FirstOrDefault();
            ViewBag.DefaultCategory = defaultCategory;
            ViewBag.isTrainer = isTrainer();


            if (!String.IsNullOrEmpty(searchString))
            {
                return View(await _context.kategoriaTreningu.Where(k => k.nazwa.Contains(searchString)).ToListAsync());
            }
            else
                return View(await _context.kategoriaTreningu.ToListAsync());
        }

        // GET: KategoriaTreningu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoriaTreningu = await _context.kategoriaTreningu.Include(k => k.treningi)
                .FirstOrDefaultAsync(m => m.id_kategorii == id);

            ViewBag.trainings = kategoriaTreningu.treningi;
            ViewBag.userId = int.Parse(User.Identity.GetUserId());

            Rola usersRole = _context.role.Include(k => k.uzytkownicy)
                                          .FirstOrDefault(m => m.nazwa == "trener");

            List<int> trainersIds = new List<int>();
            if(usersRole != null)
                foreach(var user in usersRole.uzytkownicy)
                {
                    trainersIds.Add(user.id_uzytkownika);
                }

            ViewBag.trainersIds = trainersIds;

            if (kategoriaTreningu == null)
            {
                return NotFound();
            }

            return View(kategoriaTreningu);
        }

        // GET: KategoriaTreningu/Create
        public IActionResult Create()
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            return View();
        }

        // POST: KategoriaTreningu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_kategorii,nazwa")] KategoriaTreningu kategoriaTreningu)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                _context.Add(kategoriaTreningu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategoriaTreningu);
        }

        // GET: KategoriaTreningu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!this.isTrainer())
                return RedirectToAction("Index");

            var kategoriaTreningu = await _context.kategoriaTreningu.FindAsync(id);
            if (kategoriaTreningu == null)
            {
                return NotFound();
            }
            return View(kategoriaTreningu);
        }

        // POST: KategoriaTreningu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_kategorii,nazwa")] KategoriaTreningu kategoriaTreningu)
        {
            if (id != kategoriaTreningu.id_kategorii)
            {
                return NotFound();
            }

            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategoriaTreningu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriaTreninguExists(kategoriaTreningu.id_kategorii))
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
            return View(kategoriaTreningu);
        }

        // GET: KategoriaTreningu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            var kategoriaTreningu = await _context.kategoriaTreningu
                .FirstOrDefaultAsync(m => m.id_kategorii == id);
            if (kategoriaTreningu == null)
            {
                return NotFound();
            }

            return View(kategoriaTreningu);
        }

        // POST: KategoriaTreningu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            KategoriaTreningu defaultCategory = _context.kategoriaTreningu
                                                         .Where(k => k.nazwa == "inne")
                                                         .FirstOrDefault();
            if (defaultCategory.id_kategorii == id)
                return RedirectToAction("Index");

            List<Trening> trainings = _context.treningi.Where(k => k.id_kategorii == id).ToList();

            foreach (Trening training in trainings)
                training.id_kategorii = defaultCategory.id_kategorii;

            _context.SaveChanges();

            var kategoriaTreningu = await _context.kategoriaTreningu.FindAsync(id);
            _context.kategoriaTreningu.Remove(kategoriaTreningu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategoriaTreninguExists(int id)
        {
            return _context.kategoriaTreningu.Any(e => e.id_kategorii == id);
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
