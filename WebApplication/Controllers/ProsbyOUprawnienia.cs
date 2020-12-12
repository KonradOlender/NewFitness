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
            var myContext = _context.prosbyOUprawnienia.Include(r => r.rola).Include(r => r.uzytkownik);

            return View(await myContext.ToListAsync());
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


    }
}
