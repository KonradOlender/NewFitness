using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using WebApplication.Data;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MyContext _context;


        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult TestImage()
        {
            if(!_context.obrazyTreningow.Any()) return View();
            ObrazyTreningu image = _context.obrazyTreningow.SingleOrDefault(t => t.id_obrazu==1);
            ViewBag.ImageDataUrl = image.GetImageDataUrl();
            return View();
        }

        [HttpPost, ActionName("UploadImage")]
        [ValidateAntiForgeryToken]
        public IActionResult UploadImage()
        {
            var file = Request.Form.Files[0];
            ObrazyTreningu image = new ObrazyTreningu();
            image.id_treningu = 1;

            MemoryStream memeoryStream = new MemoryStream();
            file.CopyTo(memeoryStream);
            image.obraz = memeoryStream.ToArray();

            memeoryStream.Close();
            memeoryStream.Dispose();

            _context.obrazyTreningow.Add(image);
            _context.SaveChanges();

            return View("TestImage");
        }



        public IActionResult Index()
        {                
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
