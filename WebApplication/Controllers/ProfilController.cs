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
            isAdmin();
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
                    var rate = new Ocena();
                    rate.ocena = 0;
                    ViewBag.ocenaTrener = rate;
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
                    var rate = new Ocena();
                    rate.ocena = 0;
                    ViewBag.ocenaDietetyk = rate;
                }
            }

            ViewBag.id = id;
            ViewBag.user = user;
            
            var usersProfile = _context.uzytkownicy.Where(k => k.Id == id)
                                        .Include(k => k.oceny)
                                        .Include(k => k.profilowe);
            ViewBag.profil = usersProfile;

            if (ViewBag.profil == null) return RedirectToAction("Index");
            ViewBag.posilki = await _context.posilki.Where(e => e.id_uzytkownika == id).Include(k => k.obrazy).ToListAsync();
            ViewBag.treningi = await _context.treningi.Where(e => e.id_uzytkownika == id).Include(k => k.obrazy).ToListAsync();

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
                Console.Write(e.ToString());
                return RedirectToAction("Index");
            }
            isAdmin();
            return View();
        }

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

            isAdmin();
            return RedirectToAction(nameof(Details));
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
            isAdmin();
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

            String fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension.StartsWith(".") && new List<string>() { ".png", ".jpg", ".svg" }.Contains(fileExtension))
            {
                fileExtension = fileExtension.Substring(1).ToLower();
            }
            else
            {
                ViewBag.Message = "Nieprawidłowy format pliku, akceptowane: png, jpg, svg";
                return View("AddImage");
            }

            ObrazProfilowe image;
            bool alreadyExists = await _context.obrazyProfilowe.AnyAsync(t => t.id_uzytkownika == id);
            if (alreadyExists)
                image = await _context.obrazyProfilowe.FirstAsync(t => t.id_uzytkownika == id);
            else
            {
                image = new ObrazProfilowe();
                image.id_uzytkownika = id;
            }

            image.format = fileExtension;
            MemoryStream memeoryStream = new MemoryStream();
            file.CopyTo(memeoryStream);
            image.obraz = memeoryStream.ToArray();

            memeoryStream.Close();
            memeoryStream.Dispose();

            if (alreadyExists)
                _context.Update(image);
            else
                await _context.obrazyProfilowe.AddAsync(image);
            await _context.SaveChangesAsync();
            ViewBag.Message = "Obraz został dodany";
            isAdmin();
            return View("AddImage");
        }

        private bool userExists(int user)
        {
            return _context.uzytkownicy.Any(k => k.Id == user);
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
