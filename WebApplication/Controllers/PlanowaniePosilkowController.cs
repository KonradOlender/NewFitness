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
    public class PlanowaniePosilkowController : Controller
    {
        private readonly MyContext _context;

        public PlanowaniePosilkowController(MyContext context)
        {
            _context = context;
        }

        // GET: PlanowaniePosilkow
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            var myContext = _context.planowanePosilki.Include(p => p.posilek)
                                                     .Where(x => x.id_uzytkownika == userId);
            this.isAdmin();
            return View(await myContext.ToListAsync());
        }

        // GET: PlanowaniePosilkow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userId = int.Parse(User.Identity.GetUserId());
            var planningMeals = await _context.planowanePosilki
                .Include(p => p.posilek)
                .Include(p => p.uzytkownik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (planningMeals == null)
            {
                return NotFound();
            }

            if (planningMeals.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            this.isAdmin();
            return View(planningMeals);
        }


        // GET: PlanowaniePosilkow/Create
        public IActionResult Create(string id_string)
        {
            ViewData["id_posilku"] = new SelectList(_context.posilki, "id_posilku", "nazwa");
            if (!String.IsNullOrEmpty(id_string))
            {
                int id = int.Parse(id_string);
                PlanowaniePosilkow meal = new PlanowaniePosilkow();
                meal.id_posilku = id;
                meal.data = DateTime.Now;

                //polecany posilek
                int id_recommended = this.PolecanyPosilek(meal.data);
                if(id_recommended != -1)
                ViewBag.polecany = _context.posilki.FirstOrDefault(x => x.id_posilku == id_recommended);

                this.isAdmin();
                return View(meal);
            }

            return View();
        }

        // POST: PlanowaniePosilkow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_uzytkownika,id_posilku,data")] PlanowaniePosilkow plannedMeal)
        {
            if (ModelState.IsValid)
            {
                plannedMeal.id_uzytkownika = int.Parse(User.Identity.GetUserId());
                _context.Add(plannedMeal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_posilku"] = new SelectList(_context.posilki, "id_posilku", "nazwa", plannedMeal.id_posilku);
            this.isAdmin();
            return View(plannedMeal);
        }

        // GET: PlanowaniePosilkow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plannedMeal = await _context.planowanePosilki
                .Include(p => p.posilek)
                .Include(p => p.uzytkownik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (plannedMeal == null)
            {
                return NotFound();
            }

            if (plannedMeal.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            this.isAdmin();
            return View(plannedMeal);
        }

        public IActionResult Polecany()
        {
            var recommended_id = PolecanyPosilek(DateTime.Now.Date);
            Posilek recommended = _context.posilki.First();
            if (recommended_id != -1)
            {
                recommended = _context.posilki.Single(e => e.id_posilku == recommended_id);
            }

            ViewBag.mealDetails = _context.posilekSzczegoly.Where(k => k.id_posilku == recommended_id)
                                        .Include(k => k.skladnik)
                                        .ToList();

            ViewBag.polecany = recommended;
            this.isAdmin();
            return View();
        }

        [HttpPost]
        public IActionResult Polecany(int i)
        {
            int userid = int.Parse(User.Identity.GetUserId());
            var user = _context.uzytkownicy.Single(e => e.Id == userid);

            var recommended_id = PolecanyPosilek(DateTime.Now.Date);
            Posilek recommended = _context.posilki.First();
            if (recommended_id != -1)
            {
                recommended = _context.posilki.Single(e => e.id_posilku == recommended_id);
            }

            PlanowaniePosilkow plannedMeal = new PlanowaniePosilkow();

            plannedMeal.data = DateTime.Now;
            plannedMeal.posilek = recommended;
            plannedMeal.uzytkownik = user;
            plannedMeal.id_posilku = recommended.id_posilku;
            plannedMeal.id_uzytkownika = user.Id;

            _context.Add(plannedMeal);
            _context.SaveChanges();

            this.isAdmin();
            return RedirectToAction(nameof(Index));
        }

        // POST: PlanowaniePosilkow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planningMeals = await _context.planowanePosilki.FindAsync(id);

            if (planningMeals.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            _context.planowanePosilki.Remove(planningMeals);
            await _context.SaveChangesAsync();
            this.isAdmin();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanowaniePosilkowExists(int id)
        {
            return _context.planowanePosilki.Any(e => e.id_posilku == id);
        }

        //polecane posilki - najpopulrniejsze danego dnia
        //zwraca indeks najpopularniejszego posilku lub -1 jeśli nie ma żadnych zaplanowanych posiłków na dany dzień
        private int PolecanyPosilek(DateTime date)
        {
            int count = 0, max = -1;
            var meals = _context.planowanePosilki.Where(x => x.data.Date == date.Date);
            List<PlanowaniePosilkow> list = meals.ToList();

            if (list.Count() < 1) return -1;

            foreach (PlanowaniePosilkow p in list)
            {
                if (p.id_posilku > count) count = p.id_posilku;
            }
            int[] tab = new int[count + 1];
            foreach (PlanowaniePosilkow p in list)
            {
                tab[p.id_posilku]++;
            }
            max = tab.Max();
            for(int i=0; i<tab.Count();i++)
            {
                if(tab[i]==max) return i;
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
