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
    public class ProsbyOUprawnieniaController : Controller
    {
        private readonly MyContext _context;

        public ProsbyOUprawnieniaController(MyContext context)
        {
            _context = context;
        }

        // GET: ProsbyOUprawnienia
        public async Task<IActionResult> Index()
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }
            ViewBag.allRequests = _context.prosbyOUprawnienia.Include(k => k.rola).Include(k => k.uzytkownik).ToList();
            return View();
        }


        // GET: ProsbyOUprawnienia/Create
        public IActionResult Create()
        {
            ViewData["id_roli"] = new SelectList(_context.role, "id_roli", "nazwa");
            return View();

        }

        // POST: ProsbyOUprawnienia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_roli,prosba_pisemna")] ProsbyOUprawnienia prosbyOUprawnienia)
        {

            if (RequestAlreadySent(int.Parse(User.Identity.GetUserId()), prosbyOUprawnienia.id_roli))
                return View("AlreadyExists");

            prosbyOUprawnienia.id_uzytkownika = int.Parse(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                _context.Add(prosbyOUprawnienia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["id_roli"] = new SelectList(_context.role, "id_roli", "nazwa", prosbyOUprawnienia.id_roli);
            return View(prosbyOUprawnienia);
        }

        private bool RequestAlreadySent(int user_id, int role_id)
        {
            return _context.prosbyOUprawnienia.Any(e => e.id_uzytkownika == user_id && e.id_roli == role_id);
        }

        //BŁAD 405 PRZY USUWANIU I AKCEPTOWANIU
        // POST: ProsbyOUprawnienia/Delete/5/6
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int idt, int idc)
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }

            var prosba =  _context.prosbyOUprawnienia
                .FirstOrDefault(k => k.id_uzytkownika == idt && k.id_roli == idc);
            if (prosba == null)
                return RedirectToAction("Index");

            _context.prosbyOUprawnienia.Remove(prosba);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: ProsbyOUprawnienia/Accept/5/6
        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int idt, int idc)
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }

            var prosba = _context.prosbyOUprawnienia
                .FirstOrDefault(k => k.id_uzytkownika == idt && k.id_roli == idc);
            if (prosba == null)
                return RedirectToAction("Index");

            //dodanie zaakceptowanej roli do tabeli RolaUzytkownika
            var uzytkownik = _context.uzytkownicy.FirstOrDefault(k => k.Id == idt);
            var rola = _context.role.FirstOrDefault(k => k.id_roli == idc);
            RolaUzytkownika rolaUzytkownika = new RolaUzytkownika();
            rolaUzytkownika.id_uzytkownika = idt;
            rolaUzytkownika.id_roli = idc;
            rolaUzytkownika.uzytkownik = uzytkownik;
            rolaUzytkownika.rola = rola;
            _context.Add(rolaUzytkownika);

            _context.prosbyOUprawnienia.Remove(prosba);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
