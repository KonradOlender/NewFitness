using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    public class TreningController : Controller
    {
        private readonly MyContext _context;

        public TreningController(MyContext context)
        {
            _context = context;
        }

        // GET: Trening
        public async Task<IActionResult> Index(String searchString, String category)
        {
            ViewData["currentSearchString"] = searchString;

            ViewBag.userId = int.Parse(User.Identity.GetUserId());

            Rola usersRole = _context.role.Include(k => k.uzytkownicy)
                                          .FirstOrDefault(m => m.nazwa == "trener");

            List<int> trainersIds = new List<int>();
            if(usersRole != null)
                foreach (var user in usersRole.uzytkownicy)
                {
                    trainersIds.Add(user.id_uzytkownika);
                }

            ViewBag.trainersIds = trainersIds;

            SelectList categories = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa");
            List<SelectListItem> _categories = categories.ToList();
            _categories.Insert(0, new SelectListItem() { Value = "-1", Text = "Wszystkie" });
            ViewBag.category = new SelectList((IEnumerable<SelectListItem>)_categories, "Value", "Text");

            var trainings = _context.treningi.Where(k => true);

            if (!String.IsNullOrEmpty(searchString))
            {
                trainings = trainings.Where(k => k.nazwa.Contains(searchString));
            }
            
            if(!String.IsNullOrEmpty(category))
            {
                int category_id = int.Parse(category);
                if(category_id != -1)
                    trainings = trainings.Where(k => k.id_kategorii == category_id);
            }
                
            return View(await trainings.Include(t => t.kategoria).Include(t => t.uzytkownik).ToListAsync());

        }

        // GET: Trening/Details/5
        public async Task<IActionResult> Details(int? id)
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

            if (trening == null)
            {
                return NotFound();
            }

            return View(trening);
        }

        // GET: Trening/Create
        public IActionResult Create()
        {
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa");
            return View();
        }

        // POST: Trening/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_treningu,nazwa,id_kategorii")] Trening trening)
        {
            trening.id_uzytkownika = int.Parse(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                _context.Add(trening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa", trening.id_kategorii);
            return View(trening);
        }

        // GET: Trening/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.treningi.FindAsync(id);
            if (trening == null)
            {
                return NotFound();
            }
            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            ViewData["id_kategorii"] = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa", trening.id_kategorii);
            return View(trening);
        }

        // POST: Trening/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_treningu,nazwa,id_kategorii")] Trening trening)
        {
            if (id != trening.id_treningu)
            {
                return NotFound();
            }

            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreningExists(trening.id_treningu))
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
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa", trening.id_kategorii);
            return View(trening);
        }

        // GET: Trening/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.treningi
                .Include(t => t.kategoria)
                .Include(t => t.uzytkownik)
                .FirstOrDefaultAsync(m => m.id_treningu == id);
            if (trening == null)
            {
                return NotFound();
            }

            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            return View(trening);
        }

        // POST: Trening/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trening = await _context.treningi.FindAsync(id);

            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            _context.treningi.Remove(trening);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Trening/DeleteExercise/5/6
        [HttpPost, ActionName("DeleteExercise")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExercise(int idt,int idc)
        {
            var cwiczenie = _context.treningSzczegoly.Include(k => k.trening)
                .FirstOrDefault(k => k.id_treningu == idt && k.id_cwiczenia == idc);
            if (cwiczenie == null)
                return RedirectToAction("Index");

            if (cwiczenie.trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = cwiczenie.id_treningu });

            _context.treningSzczegoly.Remove(cwiczenie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = idt});
        }

        // GET: Trening/AddExercise/5       
        public async Task<IActionResult> AddExercise(int? id, string category)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.treningi.FindAsync(id);
            if (trening == null)
            {
                return NotFound();
            }
            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            TreningSzczegoly tszczegoly = new TreningSzczegoly();
            tszczegoly.id_treningu = trening.id_treningu;
            ViewBag.trainingId = id;
            
            SelectList categories = new SelectList(_context.kategoriaCwiczenia, "id_kategorii", "nazwa");
            List<SelectListItem> _categories = categories.ToList();
            _categories.Insert(0, new SelectListItem() { Value = "-1", Text = "Wszystkie" });
            ViewBag.category = new SelectList((IEnumerable<SelectListItem>)_categories, "Value", "Text");

            if (!String.IsNullOrEmpty(category) && category != "-1")
            {
                int idc = int.Parse(category);
                ViewData["id_cwiczenia"] = new SelectList(_context.cwiczenia.Where(k => k.id_kategorii == idc), "id_cwiczenia", "nazwa");
            }
            else
                ViewData["id_cwiczenia"] = new SelectList(_context.cwiczenia, "id_cwiczenia", "nazwa");

            return View(tszczegoly);
        }

        // POST: Trening/AddExercise/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExercise(int id, [Bind("id_cwiczenia,liczba_powtorzen")] TreningSzczegoly tszczegoly)
        {
            Trening trening = _context.treningi.FirstOrDefault(x => x.id_treningu == id);
            if(trening == null)
            {
                return NotFound();
            }
            /*if (id != trening.id_treningu)
            {
                return NotFound();
            }*/

            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            tszczegoly.id_treningu = id;
            if(tszczegoly.liczba_powtorzen <= 0)
            {
                tszczegoly.liczba_powtorzen = 1;
            }

            if (ModelState.IsValid)
            {
                _context.Add(tszczegoly);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddExercise), new { id = trening.id_treningu});
            }
            ViewBag.trainingId = id;
            ViewData["id_cwiczenia"] = new SelectList(_context.cwiczenia, "id_cwiczenia", "nazwa", tszczegoly.id_cwiczenia);
            return View(tszczegoly);
        }

        private bool TreningExists(int id)
        {
            return _context.treningi.Any(e => e.id_treningu == id);
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
////////////////////////////////////////////////////////////
// Adding an exercise -> think how to add it 