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
    public class HistoriaController : Controller
    {
        private readonly MyContext _context;

        public HistoriaController(MyContext context)
        {
            _context = context;
        }

        // GET: Historia
        public async Task<IActionResult> Index()
        {
            int users_id = int.Parse(User.Identity.GetUserId());
            var myContext = _context.historiaUzytkownika.Where(k => k.id_uzytkownika==users_id);
            return View(await myContext.ToListAsync());
        }

        public async Task<IActionResult> Done()
        {

            return View();
        }

        // GET: Historia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Historia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_historia,data,waga,wzrost")] HistoriaUzytkownika historiaUzytkownika)
        {
            int users_id = int.Parse(User.Identity.GetUserId());
            if (this.HistoriaUzytkownikaExists(users_id, historiaUzytkownika.data))
                return View("AlreadyExists");                                                              

            if (ModelState.IsValid)
            {
                historiaUzytkownika.id_uzytkownika = users_id;
                _context.Add(historiaUzytkownika);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Done));
            }
            return View(historiaUzytkownika);
        }

        // GET: Historia/Edit/5
        /*public async Task<IActionResult> Edit(DateTime date)
        {
            if (date == null)
            {
                return NotFound();
            }
            int users_id = int.Parse(User.Identity.GetUserId());
            var historiaUzytkownika =  _context.historiaUzytkownika.Where(k => k.id_uzytkownika == users_id && k.data == date);
            if (historiaUzytkownika == null)
            {
                return NotFound();
            }
            return View(historiaUzytkownika);
        }

        // POST: Historia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime date, [Bind("id_historia,waga,wzrost")] HistoriaUzytkownika historiaUzytkownika)
        {
            if (date != historiaUzytkownika.data)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historiaUzytkownika);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoriaUzytkownikaExists(historiaUzytkownika.id_uzytkownika))
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
            return View(historiaUzytkownika);
        }*/

        // GET: Historia/Delete/5
        public async Task<IActionResult> Delete(String date)
        {
            if (date == null)
            {
                return NotFound();
            }
            int users_id = int.Parse(User.Identity.GetUserId());
            var historiaUzytkownika = await _context.historiaUzytkownika
                .FirstOrDefaultAsync(m => m.id_uzytkownika == users_id && m.data == Convert.ToDateTime(date));
            if (historiaUzytkownika == null)
            {
                return View("UnableToDelete");
            }
            ViewBag.date = historiaUzytkownika.data;
            return View(historiaUzytkownika);
        }

        // POST: Historia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(String date)
        {
            int users_id = int.Parse(User.Identity.GetUserId());
            var historiaUzytkownika = await _context.historiaUzytkownika
                .FirstOrDefaultAsync(m => m.id_uzytkownika == users_id && m.data == Convert.ToDateTime(date));
            if (historiaUzytkownika == null)
            {
                return View("UnableToDelete");
            }
            _context.historiaUzytkownika.Remove(historiaUzytkownika);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoriaUzytkownikaExists(int id)
        {
            return _context.historiaUzytkownika.Any(e => e.id_uzytkownika == id);
        }

        private bool HistoriaUzytkownikaExists(int id, DateTime date)
        {
            return _context.historiaUzytkownika.Any(e => e.id_uzytkownika == id && e.data == date);
        }
    }
}
