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
                return RedirectToAction(nameof(Create));
            }
            ViewBag.allRequests = await _context.prosbyOUprawnienia.Include(k => k.rola).Include(k => k.uzytkownik).ToListAsync();
            return View();
        }


        // GET: ProsbyOUprawnienia/Create
        public IActionResult Create()
        {
            ViewData["id_roli"] = new SelectList(_context.role, "id_roli", "nazwa");
            isAdmin();
            return View();

        }

        // POST: ProsbyOUprawnienia/Create
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
            isAdmin();
            return View(prosbyOUprawnienia);
        }

        private bool RequestAlreadySent(int user_id, int role_id)
        {
            return _context.prosbyOUprawnienia.Any(e => e.id_uzytkownika == user_id && e.id_roli == role_id);
        }

        // POST: ProsbyOUprawnienia/Delete/5/6
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int idu, int idr)
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }

            var prosba = _context.prosbyOUprawnienia
                .FirstOrDefault(k => k.id_uzytkownika == idu && k.id_roli == idr);
            if (prosba == null)
                return RedirectToAction("Index");

            _context.prosbyOUprawnienia.Remove(prosba);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: ProsbyOUprawnienia/Accept/5/6
        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int idu, int idr)
        {
            if (!isAdmin())
            {
                ViewBag.roleName = "admin";
                return View("UnableToAccessThisPage");
            }

            var prosba = _context.prosbyOUprawnienia
                .FirstOrDefault(k => k.id_uzytkownika == idu && k.id_roli == idr);
            if (prosba == null)
                return RedirectToAction("Index");

            //dodanie zaakceptowanej roli do tabeli RolaUzytkownika
            var uzytkownik = _context.uzytkownicy.FirstOrDefault(k => k.Id == idu);
            var rola = _context.role.FirstOrDefault(k => k.id_roli == idr);
            RolaUzytkownika rolaUzytkownika = new RolaUzytkownika();
            rolaUzytkownika.id_uzytkownika = idu;
            rolaUzytkownika.id_roli = idr;
            rolaUzytkownika.uzytkownik = uzytkownik;
            rolaUzytkownika.rola = rola;
            _context.Add(rolaUzytkownika);
            await _context.SaveChangesAsync();

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
                {
                    ViewBag.ifAdmin = true;
                    return true;
                }
            return false;
        }
    }
}
