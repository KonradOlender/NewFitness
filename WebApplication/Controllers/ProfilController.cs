using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Areas.Identity.Data;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    public class ProfilController : Controller
    {
        private readonly MyContext _context;
        public ProfilController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }

        private bool userExists(int user)
        {
            return _context.uzytkownicy.Any(k => k.Id == user);
        }

        private bool isTrainer(int user)
        {
            Rola role = _context.role.FirstOrDefault(k => k.nazwa == "trener");
            return _context.RolaUzytkownika.Any(k => k.id_uzytkownika == user && k.id_roli == role.id_roli);
        }

        private bool isDietician(int user)
        {
            Rola role = _context.role.FirstOrDefault(k => k.nazwa == "dietetyk");
            return _context.RolaUzytkownika.Any(k => k.id_uzytkownika == user && k.id_roli == role.id_roli);
        }

        // returns -1 when user isn't a trainer
        // returns -2 when user doesn't exist
        private double trainersRating(int user)
        {
            if (!userExists(user)) return -2;
            if (!isTrainer(user)) return -1;

            Rola role = _context.role.FirstOrDefault(k => k.nazwa == "trener");
            /*double ratings_sum = _context.oceny
                                      .Where(k => k.id_uzytkownika_ocenianego == user && k.id_roli == role.id_roli)
                                      .Sum(k => k.ocena);
            double ratings_count = _context.oceny
                                      .Where(k => k.id_uzytkownika_ocenianego == user && k.id_roli == role.id_roli)
                                      .Count();
            return ratings_sum / (double)ratings_count;*/
            double ratings_avg = _context.oceny
                                      .Where(k => k.id_uzytkownika_ocenianego == user && k.id_roli == role.id_roli)
                                      .Average(k => k.ocena);
            return ratings_avg;
        }

        // returns -2 when user doesn't exist
        // return -1 when user isn't a dietician
        private double dieticianRating(int user)
        {
            if (!userExists(user)) return -2;
            if (!isDietician(user)) return -1;

            Rola role = _context.role.FirstOrDefault(k => k.nazwa == "dietetyk");
            double ratings_avg = _context.oceny
                                      .Where(k => k.id_uzytkownika_ocenianego == user && k.id_roli == role.id_roli)
                                      .Average(k => k.ocena);
            return ratings_avg;
        }

        private void getListOfRatingsTrainers()
        {
            Dictionary<Uzytkownik, double> listOfRatings = new Dictionary<Uzytkownik, double>();
            Rola role = _context.role.FirstOrDefault(k => k.nazwa == "trener");
            var listOfUsers = _context.RolaUzytkownika.Include(k => k.uzytkownik).Where(k => k.id_roli == role.id_roli);
            foreach(var usersRole in listOfUsers)
            {
                listOfRatings.Add(usersRole.uzytkownik,
                                    trainersRating(usersRole.id_uzytkownika));
            }
        }

        private void getListOfRatingsDieticians()
        {
            Dictionary<Uzytkownik, double> listOfRatings = new Dictionary<Uzytkownik, double>();
            Rola role = _context.role.FirstOrDefault(k => k.nazwa == "dietetyk");
            var listOfUsers = _context.RolaUzytkownika.Include(k => k.uzytkownik).Where(k => k.id_roli == role.id_roli);
            foreach (var usersRole in listOfUsers)
            {
                listOfRatings.Add(usersRole.uzytkownik,
                                    dieticianRating(usersRole.id_uzytkownika));
            }
        }
    }
}
