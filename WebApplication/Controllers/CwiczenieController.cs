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
    public class CwiczenieController : Controller
    {
        private readonly MyContext _context;

        public CwiczenieController(MyContext context)
        {
            _context = context;
        }

        // GET: Cwiczenie
        //sort == 0 and other data arent sorted
        //sort == 1 data are sorted ascending
        //sort == 2 data are sorted descending
        public async Task<IActionResult> Index(String searchString, String category, int sort)
        {
            ViewData["currentSearchString"] = searchString;
            ViewData["currentSort"] = (sort + 1)%3;

            ViewBag.isTrainer = isTrainer();
            var exercises = _context.cwiczenia.Where(k => true);

            SelectList categories = new SelectList(_context.kategoriaCwiczenia, "id_kategorii", "nazwa");
            List<SelectListItem> _categories = categories.ToList();
            _categories.Insert(0, new SelectListItem() { Value = "-1", Text = "Wszystkie" });
            ViewBag.category = new SelectList((IEnumerable<SelectListItem>)_categories, "Value", "Text");

            if (!String.IsNullOrEmpty(category))
            {
                int id = int.Parse(category);
                if(id!= -1)
                    exercises = exercises.Where(k => k.id_kategorii == id);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                exercises = exercises.Include(c => c.kategoria).Where(k => k.nazwa.Contains(searchString));
                switch(sort)
                {
                    case 2:
                        exercises = exercises.OrderBy(k => k.spalone_kalorie);
                        break;

                    case 1:
                        exercises = exercises.OrderByDescending(k => k.spalone_kalorie);
                        break;
                    default:

                        break;
                }
                
                return View(await exercises.ToListAsync());
            }
            else
            {
                exercises = exercises.Include(c => c.kategoria).AsQueryable();
                switch (sort)
                {
                    case 2:
                        exercises = exercises.OrderBy(k => k.spalone_kalorie);
                        break;

                    case 1:
                        exercises = exercises.OrderByDescending(k => k.spalone_kalorie);
                        break;
                    default:

                        break;
                }

                return View(await exercises.ToListAsync());
            }
        }

        // GET: Cwiczenie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cwiczenie = await _context.cwiczenia
                .Include(c => c.kategoria)
                .FirstOrDefaultAsync(m => m.id_cwiczenia == id);
            if (cwiczenie == null)
            {
                return NotFound();
            }

            return View(cwiczenie);
        }

        // GET: Cwiczenie/Create
        public IActionResult Create()
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            ViewData["id_kategorii"] = new SelectList(_context.kategoriaCwiczenia, "id_kategorii", "nazwa");
            return View();
        }

        // POST: Cwiczenie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_cwiczenia,nazwa,opis,spalone_kalorie,id_kategorii")] Cwiczenie cwiczenie)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                _context.Add(cwiczenie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaCwiczenia, "id_kategorii", "nazwa", cwiczenie.id_kategorii);
            return View(cwiczenie);
        }

        // GET: Cwiczenie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            var cwiczenie = await _context.cwiczenia.FindAsync(id);
            if (cwiczenie == null)
            {
                return NotFound();
            }
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaCwiczenia, "id_kategorii", "nazwa", cwiczenie.id_kategorii);
            return View(cwiczenie);
        }

        // POST: Cwiczenie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_cwiczenia,nazwa,opis,spalone_kalorie,id_kategorii")] Cwiczenie cwiczenie)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (id != cwiczenie.id_cwiczenia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cwiczenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CwiczenieExists(cwiczenie.id_cwiczenia))
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
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaCwiczenia, "id_kategorii", "nazwa", cwiczenie.id_kategorii);
            return View(cwiczenie);
        }

        // GET: Cwiczenie/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            var cwiczenie = await _context.cwiczenia
                .Include(c => c.kategoria)
                .FirstOrDefaultAsync(m => m.id_cwiczenia == id);
            if (cwiczenie == null)
            {
                return NotFound();
            }

            return View(cwiczenie);
        }

        // POST: Cwiczenie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!this.isTrainer())
                return RedirectToAction("Index");

            var cwiczenie = await _context.cwiczenia.FindAsync(id);
            _context.cwiczenia.Remove(cwiczenie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CwiczenieExists(int id)
        {
            return _context.cwiczenia.Any(e => e.id_cwiczenia == id);
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
