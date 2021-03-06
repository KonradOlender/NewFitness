﻿using System;
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
    public class PlanowanieTreningowController : Controller
    {
        private readonly MyContext _context;

        public PlanowanieTreningowController(MyContext context)
        {
            _context = context;
        }

        // GET: PlanowanieTreningow
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            var myContext = _context.planowaneTreningi.Include(p => p.trening).Where(x => x.id_uzytkownika == userId );
            this.isAdmin();
            return View(await myContext.ToListAsync());
        }

        // GET: PlanowanieTreningow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userId = int.Parse(User.Identity.GetUserId());
            var planowanieTreningow = await _context.planowaneTreningi
                .Include(p => p.trening)
                .Include(p => p.uzytkownik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (planowanieTreningow == null)
            {
                return NotFound();
            }

            if (planowanieTreningow.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            this.isAdmin();
            return View(planowanieTreningow);
        }

        
        // GET: PlanowanieTreningow/Create
        public IActionResult Create(string id_string)
        {
            ViewData["id_treningu"] = new SelectList(_context.treningi, "id_treningu", "nazwa");
            if (!String.IsNullOrEmpty(id_string))
            {
                int id = int.Parse(id_string);
                PlanowanieTreningow training = new PlanowanieTreningow();
                training.id_treningu = id;
                training.data = DateTime.Now;
                training.notification_sent = false;

                //polecany trening
                int id_polecany = this.PolecanyTrening(training.data);
                ViewBag.idpol = id_polecany;
                if (id_polecany != -1)
                    ViewBag.polecany = _context.treningi.Where(x => x.id_treningu == id_polecany);
                else
                    ViewBag.polecany = _context.treningi.First();

                this.isAdmin();
                return View(training);
            }

            

            return View();
        }

        // POST: PlanowanieTreningow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_uzytkownika,id_treningu,data,dzien")] PlanowanieTreningow planowanieTreningow)
        {
            if (ModelState.IsValid)
            {
                planowanieTreningow.id_uzytkownika = int.Parse(User.Identity.GetUserId());
                planowanieTreningow.notification_sent = false;
                _context.Add(planowanieTreningow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_treningu"] = new SelectList(_context.treningi, "id_treningu", "nazwa", planowanieTreningow.id_treningu);
            this.isAdmin();
            return View(planowanieTreningow);
        }

        // GET: PlanowanieTreningow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planowanieTreningow = await _context.planowaneTreningi
                .Include(p => p.trening)
                .Include(p => p.uzytkownik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (planowanieTreningow == null)
            {
                return NotFound();
            }

            if (planowanieTreningow.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            this.isAdmin();
            return View(planowanieTreningow);
        }

        // POST: PlanowanieTreningow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planowanieTreningow = await _context.planowaneTreningi.FindAsync(id);

            if (planowanieTreningow.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            _context.planowaneTreningi.Remove(planowanieTreningow);
            await _context.SaveChangesAsync();
            this.isAdmin();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanowanieTreningowExists(int id)
        {
            return _context.planowaneTreningi.Any(e => e.id_treningu == id);
        }

        //polecane treningi - najpopularniejsze danego dnia
        //zwraca indeks najpopularniejszego treningu lub -1 jeśli nie ma żadnych zaplanowanych treningów na dany dzień
        private int PolecanyTrening(DateTime date)
        {
            int count = 0, max = -1;
            var training = _context.planowaneTreningi.Where(x => x.data.Date == date.Date);
            List<PlanowanieTreningow> list = training.ToList();

            if (list.Count() < 1) return -1;

            foreach (PlanowanieTreningow p in list)
            {
                if (p.id_treningu > count) count = p.id_treningu;
            }
            int[] tab = new int[count + 1];
            foreach (PlanowanieTreningow p in list)
            {
                tab[p.id_treningu]++;
            }
            max = tab.Max();
            for (int i = 0; i < tab.Count(); i++)
            {
                if (tab[i] == max) return i;
            }
            return -1;
        }
        private bool isAdmin()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.Identity.GetUserId());
                List<RolaUzytkownika> usersRoles = _context.RolaUzytkownika.Where(k => k.id_uzytkownika == userId).Include(c => c.rola).ToList();
                foreach (var usersRole in usersRoles)
                    if (usersRole.rola.nazwa == "admin")
                    {
                        ViewBag.ifAdmin = true;
                        return true;
                    }
            }
            return false;
        }
    }
}
