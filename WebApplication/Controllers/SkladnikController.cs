﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class SkladnikController : Controller
    {
        private readonly MyContext _context;

        public SkladnikController(MyContext context)
        {
            _context = context;
        }

        // GET: Skladnik
        public async Task<IActionResult> Index(String searchString, String category, int sort)
        {
            ViewData["currentSearchString"] = searchString;
            ViewData["currentSort"] = (sort + 1) % 3;

            ViewBag.isDietician = isDietician();
            var components = _context.skladnik.Where(k => true);

            SelectList categories = new SelectList(_context.kategoriaSkladnikow, "id_kategorii", "nazwa");
            List<SelectListItem> _categories = categories.ToList();
            _categories.Insert(0, new SelectListItem() { Value = "-1", Text = "Wszystkie" });
            ViewBag.category = new SelectList((IEnumerable<SelectListItem>)_categories, "Value", "Text");

            if (!String.IsNullOrEmpty(category))
            {
                int id = int.Parse(category);
                if (id != -1)
                    components = components.Where(k => k.id_kategorii == id);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                components = components.Include(c => c.kategoria).Where(k => k.nazwa.Contains(searchString));
                switch (sort)
                {
                    case 2:
                        components = components.OrderBy(k => k.waga);
                        break;

                    case 1:
                        components = components.OrderByDescending(k => k.waga);
                        break;
                    default:

                        break;
                }

                return View(await components.ToListAsync());
            }
            else
            {
                components = components.Include(c => c.kategoria).AsQueryable();
                switch (sort)
                {
                    case 2:
                        components = components.OrderBy(k => k.waga);
                        break;

                    case 1:
                        components = components.OrderByDescending(k => k.waga);
                        break;
                    default:

                        break;
                }

                return View(await components.ToListAsync());
            }
        
    }

        // GET: Skladnik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skladnik = await _context.skladnik
                .Include(s => s.kategoria)
                .FirstOrDefaultAsync(m => m.id_skladnika == id);
            if (skladnik == null)
            {
                return NotFound();
            }

            return View(skladnik);
        }

        // GET: Skladnik/Create
        public IActionResult Create()
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            ViewData["id_kategorii"] = new SelectList(_context.kategoriaSkladnikow, "id_kategorii", "nazwa");
            return View();
        }

        // POST: Skladnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_skladnika,nazwa,waga,kalorie,id_kategorii")] Skladnik skladnik)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                _context.Add(skladnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaSkladnikow, "id_kategorii", "nazwa", skladnik.id_kategorii);
            return View(skladnik);
        }

        //obliczanie kalorii posiłku 
        private static int editedwaga=-1, editedkalorie=-1;

        // GET: Skladnik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            var skladnik = await _context.skladnik.FindAsync(id);
            if (skladnik == null)
            {
                return NotFound();
            }
            editedwaga = skladnik.waga;
            editedkalorie = skladnik.kalorie;
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaSkladnikow, "id_kategorii", "nazwa", skladnik.id_kategorii);
            return View(skladnik);
        }

        // POST: Skladnik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_skladnika,nazwa,waga,kalorie,id_kategorii")] Skladnik skladnik)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            if (id != skladnik.id_skladnika)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    editCalories(false, skladnik);
                    _context.Update(skladnik);
                    editCalories(true, skladnik);

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkladnikExists(skladnik.id_skladnika))
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
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaSkladnikow, "id_kategorii", "nazwa", skladnik.id_kategorii);
            return View(skladnik);
        }

        // GET: Skladnik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            var skladnik = await _context.skladnik
                .Include(s => s.kategoria)
                .FirstOrDefaultAsync(m => m.id_skladnika == id);
            if (skladnik == null)
            {
                return NotFound();
            }

            return View(skladnik);
        }

        // POST: Skladnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!this.isDietician())
                return RedirectToAction("Index");

            var skladnik = await _context.skladnik.FindAsync(id);
            _context.skladnik.Remove(skladnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkladnikExists(int id)
        {
            return _context.skladnik.Any(e => e.id_skladnika == id);
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

        //podliczanie kalorii
        public void editCalories(bool dzialanie, Skladnik skladnik)
        {
            int waga = skladnik.waga;
            int kalorie = skladnik.kalorie;
            List<PosilekSzczegoly> posilki = _context.posilekSzczegoly.Where(k => k.id_skladnika == skladnik.id_skladnika).Include(c => c.posilek).ToList();
            foreach (PosilekSzczegoly pszczegoly in posilki)
            {
                if (dzialanie)
                {
                    //dodwanie
                    pszczegoly.posilek.kalorie += pszczegoly.porcja / waga * kalorie;
                }
                else
                {
                    //usuwanie
                    if(editedwaga>0 && editedkalorie>0) pszczegoly.posilek.kalorie -= pszczegoly.porcja / editedwaga * editedkalorie;
                }
            }
        }
    }
}
