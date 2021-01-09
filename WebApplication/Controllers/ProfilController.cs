using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using WebApplication.Data;

namespace WebApplication.Controllers
{
    public class ProfilController : Controller
    {
        private readonly MyContext _context;
        public IActionResult Index()
        {

            return View();
        }
    }
}
