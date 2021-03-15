using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        //chat 
        //czy tu IdentityUser?
        public ProfilController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }

        //musi zapamiętywać aktualną ocene
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userid = int.Parse(User.Identity.GetUserId());
            Uzytkownik user = _context.uzytkownicy.Single(e => e.Id == userid);

            if (isTrainer(id))
            {
                try
                {
                    ViewBag.ocenaTrener = _context.oceny.Single(e => e.id_uzytkownika_oceniajacego == userid && e.id_uzytkownika_ocenianego == id && e.id_roli == 2);
                }
                catch
                {
                    ViewBag.ocenaTrener = new Ocena();
                }
            }
            if (isDietician(id))
            {
                try
                {
                    ViewBag.ocenaDietetyk = _context.oceny.Single(e => e.id_uzytkownika_oceniajacego == userid && e.id_uzytkownika_ocenianego == id && e.id_roli == 3);
                }
                catch
                {
                    ViewBag.ocenaDietetyk = new Ocena();
                }
                
            }

            ViewBag.id = id;
            ViewBag.user = user;
            
            var usersProfile = _context.uzytkownicy.Where(k => k.Id == id)
                                        .Include(k => k.oceny)
                                        .Include(k => k.profilowe);
            ViewBag.profil = usersProfile;

            if (ViewBag.profil == null) return RedirectToAction("Index");
            ViewBag.posilki = _context.posilki.Where(e => e.id_uzytkownika == id).Include(k => k.obrazy).ToList();

            ViewBag.obrazyP = _context.obrazyPosilkow.ToList();
            ViewBag.obrazyT = _context.obrazyTreningow.ToList();

            ViewBag.treningi = _context.treningi.Where(e => e.id_uzytkownika == id).ToList();

            ViewBag.index = id;
            ViewBag.isTrainer = isTrainer(id);
            ViewBag.isDietician = isDietician(id);

            ViewBag.falsee = false;

            ViewBag.t_rating = trainersRating(id);
            ViewBag.d_rating = dieticianRating(id);

            try
            {
                if (usersProfile.First().profilowe == null)
                    ViewBag.image = null;
                else
                    ViewBag.image = usersProfile.First().profilowe.GetImageDataUrl();
            }
            catch(Exception e)
            {
                return RedirectToAction("Index");
            }
            

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

            if(_context.oceny.Any(e=> e.id_uzytkownika_oceniajacego == userid && e.id_uzytkownika_ocenianego == oceniany.Id))
            {
                Ocena ocena = new Ocena();

                ocena.id_uzytkownika_oceniajacego = userid;
                ocena.id_uzytkownika_ocenianego = oceniany.Id;
                ocena.ocena = rating;
                ocena.oceniajacy = user;
                ocena.oceniany = oceniany;
                ocena.rola = role;
                ocena.id_roli = role.id_roli;

                _context.Update(ocena);
            }
            else
            {
                Ocena ocena = new Ocena();

                ocena.id_uzytkownika_oceniajacego = userid;
                ocena.id_uzytkownika_ocenianego = oceniany.Id;
                ocena.ocena = rating;
                ocena.oceniajacy = user;
                ocena.oceniany = oceniany;
                ocena.rola = role;
                ocena.id_roli = role.id_roli;

                _context.Add(ocena);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Details));

            //return View();
        }

        public IActionResult AddImage(int id)
        {
            int userId = int.Parse(User.Identity.GetUserId());
            if (userId != id)
            {
                int newId = id;
                return RedirectToAction("Details", new { id = newId });
            }
            
            ViewBag.Message = "";
            ViewBag.user = true;
            return View();
        }

        [HttpPost, ActionName("AddImage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImagePost(int id)
        {
            int userId = int.Parse(User.Identity.GetUserId());
            if (userId != id)
            {
                int newId = id;
                return RedirectToAction("Details", new { id = newId });
            }
            ViewBag.user = true;
            var file = Request.Form.Files.Count != 0 ? Request.Form.Files[0] : null;
            if (file == null)
            {
                ViewBag.Message = "Nie wybrano obrazu do przesłania";
                return View("AddImage");
            }

            ObrazyTreningu image = new ObrazyTreningu();
            image.id_treningu = id;

            MemoryStream memeoryStream = new MemoryStream();
            file.CopyTo(memeoryStream);
            image.obraz = memeoryStream.ToArray();

            memeoryStream.Close();
            memeoryStream.Dispose();

            _context.obrazyTreningow.Add(image);
            _context.SaveChanges();
            ViewBag.Message = "Obraz został dodany";
            return View("AddImage");
        }


        private bool userExists(int user)
        {
            return _context.uzytkownicy.Any(k => k.Id == user);
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

        //chat
        public async Task<IActionResult> Chat()
        {
            //var currentUser = await _userManager.GetUserAsync(User);
            int userid = int.Parse(User.Identity.GetUserId());
            var currentUser = _context.uzytkownicy.Single(e => e.Id == userid);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }
            //var messages = await _context.Messages.ToListAsync();
            return View();
        }
        /*public async Task<IActionResult> Create(Messages message)
        {
            if (ModelState.IsValid)
            {
                message.UserName = User.Identity.Name;
                //var sender = await _userManager.GetUserAsync(User);
                int userid = int.Parse(User.Identity.GetUserId());
                var sender = _context.uzytkownicy.Single(e => e.Id == userid);
                message.UserID = sender.Id;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Error();
        }*/



    }
}
