using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Authorize]
    public class OdznakiController : Controller
    {
        public IActionResult Index()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            ViewBag.gold = false;
            ViewBag.silver = true;
            ViewBag.bronze = true;
            ViewBag.progressLeft = 20;
            ViewBag.progress = 80;
            return View();
        }
    }
}
