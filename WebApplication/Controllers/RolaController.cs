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
    public class RolaController : Controller
    {
        private readonly MyContext _context;

        public RolaController(MyContext context)
        {
            _context = context;
        }

        // GET: Rolas
        public async Task<IActionResult> Index()
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }
            return View(await _context.role.ToListAsync());
        }
        
        public async Task<IActionResult> MyRoles()
        {
            int user_id = int.Parse(User.Identity.GetUserId());
            ViewBag.allRoles = _context.role.ToList();
            //ViewBag.userRoles = _context.RolaUzytkownika.Include(k => k.rola).Where(k => k.id_uzytkownika == user_id).ToList();
            List<RolaUzytkownika> userRoles = _context.RolaUzytkownika.Include(k => k.rola).Where(k => k.id_uzytkownika == user_id).ToList();
            ViewBag.usersRoles = userRoles.ToDictionary<RolaUzytkownika, Rola>(k => k.rola);
            //ViewBag.userRoles = ViewBag.userRoles.ToDictionary<RolaUzytkownika, Rola>(t => t.rola)
            return View();
        }


        // GET: Rolas/Create
        public IActionResult Create()
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }

            return View();
        }

        // POST: Rolas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_roli,nazwa")] Rola rola)
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }

            if (ModelState.IsValid)
            {
                _context.Add(rola);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rola);
        }

        
        // GET: Rolas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }

            var rola = await _context.role
                .FirstOrDefaultAsync(m => m.id_roli == id);
            if (rola == null)
            {
                return NotFound();
            }

            return View(rola);
        }

        // POST: Rolas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }

            var rola = await _context.role.FindAsync(id);
            _context.role.Remove(rola);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolaExists(int id)
        {
            return _context.role.Any(e => e.id_roli == id);
        }

        private bool isAdmin()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            List<RolaUzytkownika> usersRoles = _context.RolaUzytkownika.Where(k => k.id_uzytkownika == userId).Include(c => c.rola).ToList();
            foreach (var usersRole in usersRoles)
                if (usersRole.rola.nazwa == "admin")
                    return true;
            return false;
        }
    }
}
