using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace WebApplication.Controllers
{
    public class TreningList : Controller
    {
        private readonly MyContext _context;

        public TreningList(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            var listaplantrening = _context.planowaneTreningi.Where(e => e.id_uzytkownika == userId);
            var listatrening = _context.treningi.ToList();
            ViewBag.listaplantrening = listaplantrening;
            ViewBag.listatrening = listatrening;
            return View();
        }
    }
}
