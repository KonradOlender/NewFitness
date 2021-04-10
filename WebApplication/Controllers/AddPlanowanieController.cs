using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Areas.Identity.Data;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AddPlanowanieController : Controller
    {
        private readonly MyContext _context;

        public AddPlanowanieController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var allMeals = _context.posilki.ToList();
            List<Posilek> posilki = new List<Posilek>();
            foreach (var item in allMeals)
            {
                if (isDietician(item.id_uzytkownika))
                {
                    posilki.Add(item);
                }
            }
            

            ViewBag.posilki = posilki;
            return View();
        }

        private bool isDietician(int id)
        {
            var usersRoles = _context.RolaUzytkownika.Include(e => e.rola.nazwa).Where(k => k.id_uzytkownika == id).ToList();
            foreach (var item in usersRoles)
            {
                if (item.rola.nazwa == "dietetyk")
                {
                    return true;
                }
                //sprawdza tez czy uzytkownik jest administratorem - do wyswietlnia menu
                if(item.rola.nazwa == "admin")
                    ViewBag.ifAdmin = true;
            }
            return false;
        }


    }
}
