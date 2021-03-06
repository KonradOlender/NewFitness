﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using WebApplication.Data;
using WebApplication.Areas;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using WebApplication.Entities;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MyContext _context;
        private EmailSender emailSender;
        private string emailAdress;
        private IHostingEnvironment _env;
        public HomeController(ILogger<HomeController> logger, MyContext context, 
            IOptions<EmailSettings> emailSettings, IHostingEnvironment env)
        {
            _logger = logger;
            _context = context;
            emailSender = new EmailSender(emailSettings, env);
            emailAdress = emailSettings.Value.Sender;
            _env = env;
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
            if (User.Identity.IsAuthenticated)
            {
                this.isAdmin();
                return RedirectToAction("Index", "UserHub");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            this.isAdmin();
            return View();
        }

        public IActionResult ContactUs()
        {
            ViewBag.message = "";
            ViewBag.ErrorMessage = "";
            this.isAdmin();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(string name, string subject, string message)
        {
            ViewBag.message = "Wiadomość została wysłana";
            if((name == null || name =="") || (subject == null || subject == "" ) || (message == "" || message == null))
            {
                ViewBag.ErrorMessage = "Wszystkie pola muszą zostać wypełnione";
                ViewBag.message = "";
                return View();
            }
            string filePath = Path.Combine(_env.WebRootPath, "messages/ContactMessage.html");
            string messageHtml = System.IO.File.ReadAllText(filePath);
            string messageToSent = string.Format(messageHtml, name, message);
            await emailSender.SendEmailAsync(emailAdress,"Zapytanie ze strony: " + subject, messageToSent);
            this.isAdmin();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
