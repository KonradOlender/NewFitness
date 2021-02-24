﻿using System;
using System.Collections.Generic;
using System.IO;
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
    public class PosilekController : Controller
    {
        private readonly MyContext _context;

        public PosilekController(MyContext context)
        {
            _context = context;
        }

        // GET: Posilek
        public async Task<IActionResult> Index(String searchString)
        {
            ViewData["currentSearchString"] = searchString;

            ViewBag.userId = int.Parse(User.Identity.GetUserId());

            Rola usersRole = _context.role.Include(k => k.uzytkownicy)
                                          .FirstOrDefault(m => m.nazwa == "dietetyk");

            List<int> dieticiansIds = new List<int>();
            if (usersRole != null)
                foreach (var user in usersRole.uzytkownicy)
                {
                    dieticiansIds.Add(user.id_uzytkownika);
                }

            ViewBag.dieticiansIds = dieticiansIds;


            var meals = _context.posilki.Where(k => true);

            if (!String.IsNullOrEmpty(searchString))
            {
                meals = meals.Where(k => k.nazwa.Contains(searchString));
            }

            return View(await meals.Include(t => t.uzytkownik).ToListAsync());

        }

        // GET: Posilek/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posilek = await _context.posilki
                .Include(p => p.uzytkownik)
                .Include(p => p.obrazy)
                .FirstOrDefaultAsync(m => m.id_posilku == id);

            ViewBag.mealsDetails = _context.posilekSzczegoly.Where(k => k.id_posilku == id)
                                        .Include(k => k.skladnik)
                                        .ToList();

            ViewBag.userId = int.Parse(this.User.Identity.GetUserId());
            ViewBag.posilekOwner = posilek.id_uzytkownika;

            if (posilek == null)
            {
                return NotFound();
            }

            if (posilek.obrazy.Count <= 0)
                ViewBag.image = null;
            else
                ViewBag.image = posilek.obrazy
                                    .Last()
                                    .GetImageDataUrl();

            return View(posilek);
        }

        // GET: Posilek/Create
        public IActionResult Create()
        {
            ViewData["id_uzytkownika"] = new SelectList(_context.uzytkownicy, "Id", "Id");
            return View();
        }

        // POST: Posilek/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_posilku,nazwa,opis")] Posilek posilek)
        {
            posilek.id_uzytkownika = int.Parse(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                _context.Add(posilek);
                //podliczanie kalorii
                posilek.kalorie = 0;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(posilek);
        }

        // GET: Posilek/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posilek = await _context.posilki.FindAsync(id);
            if (posilek == null)
            {
                return NotFound();
            }

            if (posilek.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = posilek.id_posilku });

            return View(posilek);
        }

        // POST: Posilek/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_posilku,nazwa,opis")] Posilek posilek)
        {
            if (id != posilek.id_posilku)
            {
                return NotFound();
            }
            Posilek posilekToEdit = await _context.posilki.FindAsync(id);

            if (posilekToEdit.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = posilek.id_posilku });

            if (ModelState.IsValid)
            {
                try
                {
                    posilekToEdit.nazwa = posilek.nazwa;
                    posilekToEdit.opis = posilek.opis;
                    _context.Update(posilekToEdit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosilekExists(posilek.id_posilku))
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

            return View(posilek);
        }

        // GET: Posilek/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posilek = await _context.posilki
                .Include(p => p.uzytkownik)
                .FirstOrDefaultAsync(m => m.id_posilku == id);
            if (posilek == null)
            {
                return NotFound();
            }

            if (posilek.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = posilek.id_posilku });

            return View(posilek);
        }

        // POST: Posilek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posilek = await _context.posilki.FindAsync(id);

            if (posilek.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = posilek.id_posilku });

            _context.posilki.Remove(posilek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Posilek/DeleteComponent/5/6
        [HttpPost, ActionName("DeleteComponent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComponent(int idt, int idc)
        {
            var skladnik = _context.posilekSzczegoly.Include(k => k.posilek)
                .FirstOrDefault(k => k.id_posilku == idt && k.id_skladnika == idc);
            if (skladnik == null)
                return RedirectToAction("Index");

            if (skladnik.posilek.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = skladnik.id_posilku });

            editCalories(false, skladnik);

            _context.posilekSzczegoly.Remove(skladnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = idt });
        }

        // GET: Posilek/AddComponent/5       
        public async Task<IActionResult> AddComponent(int? id, string category)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posilek = await _context.posilki.FindAsync(id);
            if (posilek == null)
            {
                return NotFound();
            }
            if (posilek.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = posilek.id_posilku });

            PosilekSzczegoly pszczegoly = new PosilekSzczegoly();
            pszczegoly.id_posilku = posilek.id_posilku;
            ViewBag.posilekId = id;

            SelectList categories = new SelectList(_context.kategoriaSkladnikow, "id_kategorii", "nazwa");
            List<SelectListItem> _categories = categories.ToList();
            _categories.Insert(0, new SelectListItem() { Value = "-1", Text = "Wszystkie" });
            ViewBag.category = new SelectList((IEnumerable<SelectListItem>)_categories, "Value", "Text");

            if (!String.IsNullOrEmpty(category) && category != "-1")
            {
                int idc = int.Parse(category);
                ViewData["id_skladnika"] = new SelectList(_context.skladnik.Where(k => k.id_kategorii == idc), "id_skladnika", "nazwa");
            }
            else
                ViewData["id_skladnika"] = new SelectList(_context.skladnik, "id_skladnika", "nazwa");

            return View(pszczegoly);
        }

        // POST: Posilek/AddComponent/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComponent(int id, [Bind("id_skladnika,porcja")] PosilekSzczegoly pszczegoly)
        {
            Posilek posilek = _context.posilki.FirstOrDefault(x => x.id_posilku == id);
            if (posilek == null)
            {
                return NotFound();
            }

            if (posilek.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = posilek.id_posilku });

            if (ComponentAlreadyInMeal(posilek.id_posilku, pszczegoly.id_skladnika))
                return View("AlreadyExists");

            pszczegoly.id_posilku = id;
            if (pszczegoly.porcja <= 0)
            {
                pszczegoly.porcja = 1;
            }

            if (ModelState.IsValid)
            {
                _context.Add(pszczegoly);

                editCalories(true, pszczegoly);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddComponent), new { id = posilek.id_posilku });
            }
            ViewBag.posilekId = id;
            ViewData["id_skladnika"] = new SelectList(_context.skladnik, "id_skladnika", "nazwa", pszczegoly.id_skladnika);
            return View(pszczegoly);

        }

        public IActionResult AddImage(int id)
        {
            if (!_context.posilki.Any(t => t.id_posilku == id))
            {
                ViewBag.Message = "Nie ma takiego posilku";
                ViewBag.meal = false;
                return View("AddImage");
            }

            var meal = _context.posilki.FirstOrDefault(t => t.id_posilku == id);
            if (meal.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = meal.id_posilku });

            ViewBag.Message = "";
            ViewBag.meal = true;
            return View();
        }

        [HttpPost, ActionName("AddImage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImagePost(int id)
        {
            if (!_context.posilki.Any(t => t.id_posilku == id))
            {
                ViewBag.Message = "Nie ma takiego posilku";
                ViewBag.meal = false;
                return View("AddImage");
            }

            var meal = _context.posilki.FirstOrDefault(t => t.id_posilku == id);
            if (meal.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = meal.id_posilku });

            ViewBag.Message = "";
            ViewBag.meal = true;

            var file = Request.Form.Files.Count != 0 ? Request.Form.Files[0] : null;
            if (file == null)
            {
                ViewBag.Message = "Nie wybrano obrazu do przesłania";
                return View("AddImage");
            }

            ObrazyPosilku image = new ObrazyPosilku();
            image.id_posilku = id;

            MemoryStream memeoryStream = new MemoryStream();
            file.CopyTo(memeoryStream);
            image.obraz = memeoryStream.ToArray();

            memeoryStream.Close();
            memeoryStream.Dispose();

            _context.obrazyPosilkow.Add(image);
            _context.SaveChanges();
            ViewBag.Message = "Obraz został dodany";
            return View("AddImage");
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

        private bool ComponentAlreadyInMeal(int meal_id, int component_id)
        {
            return _context.posilekSzczegoly.Any(e => e.id_posilku == meal_id && e.id_skladnika == component_id);
        }

        private bool PosilekExists(int id)
        {
            return _context.posilki.Any(e => e.id_posilku == id);
        }

        //podliczanie kalorii
        public void editCalories(bool dzialanie, PosilekSzczegoly pszczegoly)
        {
            int id_skladnika = pszczegoly.id_skladnika;
            List<Skladnik> skladniki = _context.skladnik.Where(k => k.id_skladnika == id_skladnika).ToList();
            double waga = (double)skladniki[0].waga;
            double kalorie = (double)skladniki[0].kalorie;

            if (dzialanie)
            {
                //dodwanie
                pszczegoly.posilek.kalorie += (int)((double)pszczegoly.porcja / waga * kalorie);
            }
            else
            {
                //usuwanie
                pszczegoly.posilek.kalorie -= (int)((double)pszczegoly.porcja / waga * kalorie);
            }
        }

        public int getWeglowodany(int meal_id)
        {
            List<PosilekSzczegoly> meals = _context.posilekSzczegoly.Include(t => t.skladnik).Where(t => t.id_posilku == meal_id).ToList();
            int sum = 0;
            foreach (var ps in meals)
            {
                sum += ps.skladnik.weglowodany * ps.porcja / 100;
            }
            return sum;
        }

        public int getBialko(int meal_id)
        {
            List<PosilekSzczegoly> meals = _context.posilekSzczegoly.Include(t => t.skladnik).Where(t => t.id_posilku == meal_id).ToList();
            int sum = 0;
            foreach (var ps in meals)
            {
                sum += ps.skladnik.bialko * ps.porcja / 100;
            }
            return sum;
        }

        public int getTluszcze(int meal_id)
        {
            List<PosilekSzczegoly> meals = _context.posilekSzczegoly.Include(t => t.skladnik).Where(t => t.id_posilku == meal_id).ToList();
            int sum = 0;
            foreach (var ps in meals)
            {
                sum += ps.skladnik.tluszcze * ps.porcja / 100;
            }
            return sum;
        }

        private double mealRating(int meal_id)
        {
            if (!_context.ocenyPosilkow.Any(k => k.id_posilku == meal_id))
                return 0;
            double ratings_avg = _context.ocenyPosilkow
                                      .Where(k => k.id_posilku == meal_id)
                                      .Average(k => k.ocena);
            return ratings_avg;
        }
    }
}
