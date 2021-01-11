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

        //musi zapamiętywać aktualną ocene
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.id = id;
            ViewBag.profil = _context.uzytkownicy.Where(k => k.Id == id)
                                        .Include(k => k.oceny);
            ViewBag.posilki = _context.posilki.Where(e => e.id_uzytkownika == id).ToList();
            ViewBag.treningi = _context.treningi.Where(e => e.id_uzytkownika == id).ToList();

            ViewBag.index = id;
            ViewBag.isTrainer = isTrainer(id);
            ViewBag.isDietician = isDietician(id);

            ViewBag.t_rating = trainersRating(id);
            ViewBag.d_rating = dieticianRating(id);



            return View();
        }

        //wywala jak jest już dana ocena, do zrobienia
        [HttpPost]
        public IActionResult Details(int oceniany_id, double rating, string rola)
        {
            
            int userid = int.Parse(User.Identity.GetUserId());  
            Uzytkownik user = _context.uzytkownicy.Single(e => e.Id == userid);

            var oceniany = _context.uzytkownicy.Single(e => e.Id == oceniany_id);
            var role = _context.role.Single(e=>e.nazwa == rola);

            Ocena ocena = new Ocena();

            ocena.id_uzytkownika_oceniajacego = userid;
            ocena.id_uzytkownika_ocenianego = oceniany.Id;
            ocena.ocena = rating;
            ocena.oceniajacy = user;
            ocena.oceniany = oceniany;
            ocena.rola = role;
            ocena.id_roli = role.id_roli;



            _context.Add(ocena);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details));

            //return View();
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
            if (!_context.oceny.Any(k => k.id_uzytkownika_ocenianego == user && k.id_roli == role.id_roli))
                return 0;
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
            if (!_context.oceny.Any(k => k.id_uzytkownika_ocenianego == user && k.id_roli == role.id_roli))
                return 0;
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
