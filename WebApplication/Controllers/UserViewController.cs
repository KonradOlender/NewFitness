using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Controllers
{
    public class UserViewController : Controller
    {
        private readonly MyContext _context;

        public UserViewController(MyContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var uzytkownicy = _context.uzytkownicy.ToList();
            List<Uzytkownik> trenerzy = new List<Uzytkownik>();
            List<Uzytkownik> dietetycy = new List<Uzytkownik>();

            foreach(var item in uzytkownicy)
            {
                if (isTrainer(item.Id))
                {
                    trenerzy.Add(item);
                }
                else if (isDietician(item.Id))
                {
                    dietetycy.Add(item);
                }
            }

            ViewBag.trenerzy = trenerzy;
            ViewBag.dietetycy = dietetycy;

            return View();
        }

        private bool isTrainer(int user)
        {
            if (!_context.role.Any(k => k.nazwa == "trener")) return false;
            Rola role = _context.role.FirstOrDefault(k => k.nazwa == "trener");
            return _context.RolaUzytkownika.Any(k => k.id_uzytkownika == user && k.id_roli == role.id_roli);
        }
 
        private bool isDietician(int user)
        {
            if (!_context.role.Any(k => k.nazwa == "dietetyk")) return false;
            Rola role = _context.role.FirstOrDefault(k => k.nazwa == "dietetyk");
            return _context.RolaUzytkownika.Any(k => k.id_uzytkownika == user && k.id_roli == role.id_roli);
        }
    }

}
