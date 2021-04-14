using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Areas.Identity.Data;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    public class OdznakiController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public OdznakiController(IWebHostEnvironment hostingEnvironment, MyContext context)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            var user = _context.uzytkownicy.FirstOrDefault(t => t.Id == userId);
            HistoriaUzytkownika usersHistory = _context.historiaUzytkownika.Where(t => t.id_uzytkownika == userId)
                                                                            .OrderByDescending(t => t.data).FirstOrDefault();

            double progress = Math.Abs(user.cel - usersHistory.waga)*100 / user.cel;
            if (user.cel - usersHistory.waga < 0) progress = 100;
            if ((double)Odznaki.gold > progress)
                ViewBag.gold = false;
            else
                ViewBag.gold = true;

            if ((double)Odznaki.silver > progress)
                ViewBag.silver = false;
            else
                ViewBag.silver = true;

            if ((double)Odznaki.bronze > progress)
                ViewBag.bronze = false;
            else
                ViewBag.bronze = true;

            ViewBag.progressLeft = 100 - (int)progress;
            ViewBag.progress = progress;
            this.isAdmin();
            return View();
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
