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
    public class RolaUzytkownikaController : Controller
    {
        private readonly MyContext _context;

        public RolaUzytkownikaController(MyContext context)
        {
            _context = context;
        }

        // GET: RolaUzytkownika
        public async Task<IActionResult> Index()
        {
            var myContext = _context.RolaUzytkownika.Include(r => r.rola).Include(r => r.uzytkownik);
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }
                
            return View(await myContext.Include(k => k.uzytkownik).ToListAsync());
        }

        // GET: RolaUzytkownika/Create
        public IActionResult Create()
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }
            ViewData["id_roli"] = new SelectList(_context.role, "id_roli", "nazwa");
            ViewData["id_uzytkownika"] = new SelectList(_context.uzytkownicy, "Id", "login");
            return View();
        }

        // POST: RolaUzytkownika/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_uzytkownika,id_roli")] RolaUzytkownika rolaUzytkownika)
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }

            if (ModelState.IsValid)
            {
                _context.Add(rolaUzytkownika);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_roli"] = new SelectList(_context.role, "id_roli", "nazwa", rolaUzytkownika.id_roli);
            ViewData["id_uzytkownika"] = new SelectList(_context.uzytkownicy, "Id", "login", rolaUzytkownika.id_uzytkownika);
            return View(rolaUzytkownika);
        }

        // POST: RolaUzytkownika/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int role_id, int users_id)
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return Redirect("UnableToAccessThisPage");
            }
            var rolaUzytkownika =  _context.RolaUzytkownika.FirstOrDefault(k => k.id_roli == role_id && k.id_uzytkownika == users_id);
            if(rolaUzytkownika != null)
                _context.RolaUzytkownika.Remove(rolaUzytkownika);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolaUzytkownikaExists(int id)
        {
            return _context.RolaUzytkownika.Any(e => e.id_roli == id);
        }

        private bool isAdmin()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            List<RolaUzytkownika> usersRoles = _context.RolaUzytkownika.Where(k => k.id_uzytkownika == userId).Include(c => c.rola).ToList();
            foreach (var usersRole in usersRoles)
                if (usersRole.rola.nazwa == "admin")
                {
                    ViewBag.ifAdmin = true;
                    return true;
                }
            return false;
        }
    }
}
