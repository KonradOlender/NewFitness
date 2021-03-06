﻿using System;
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

namespace WebApplication.Controllers
{
    [Authorize]
    public class TreningController : Controller
    {
        private readonly MyContext _context;

        public TreningController(MyContext context)
        {
            _context = context;
        }

        // GET: Trening
        public async Task<IActionResult> Index(String searchString, String category, int sort)
        {
            ViewData["currentSearchString"] = searchString;
            ViewData["currentSort"] = (sort + 1) % 3;

            ViewBag.userId = int.Parse(User.Identity.GetUserId());

            Rola usersRole = _context.role.Include(k => k.uzytkownicy)
                                          .FirstOrDefault(m => m.nazwa == "trener");

            List<int> trainersIds = new List<int>();
            if(usersRole != null)
                foreach (var user in usersRole.uzytkownicy)
                {
                    trainersIds.Add(user.id_uzytkownika);
                }

            ViewBag.trainersIds = trainersIds;

            SelectList categories = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa");
            List<SelectListItem> _categories = categories.ToList();
            _categories.Insert(0, new SelectListItem() { Value = "-1", Text = "Wszystkie" });
            ViewBag.category = new SelectList((IEnumerable<SelectListItem>)_categories, "Value", "Text");

            var trainings = _context.treningi.Where(k => true);

            if (!String.IsNullOrEmpty(searchString))
            {
                trainings = trainings.Where(k => k.nazwa.Contains(searchString));
            }
            
            if(!String.IsNullOrEmpty(category))
            {
                int category_id = int.Parse(category);
                if(category_id != -1)
                    trainings = trainings.Where(k => k.id_kategorii == category_id);
            }

            switch (sort)
            {
                case 2:
                    ViewBag.ratingsCounted = _context.ocenyTreningow.GroupBy(t => t.id_treningu)
                                                            .Select(t => new { training = t.Key, Avg = t.Average(k => k.ocena) }).OrderBy(t => t.Avg)
                                                            .AsEnumerable()
                                                            .ToDictionary(k => k.training, v => v.Avg);
                    break;

                case 1:
                    ViewBag.ratingsCounted = _context.ocenyTreningow.GroupBy(t => t.id_treningu)
                                                            .Select(t => new { training = t.Key, Avg = t.Average(k => k.ocena) }).OrderByDescending(t => t.Avg)
                                                            .AsEnumerable()
                                                            .ToDictionary(k => k.training, v => v.Avg);
                    break;
                default:
                    ViewBag.ratingsCounted = _context.ocenyTreningow.GroupBy(t => t.id_treningu)
                                                            .Select(t => new { training = t.Key, Avg = t.Average(k => k.ocena) })
                                                            .AsEnumerable()
                                                            .ToDictionary(k => k.training, v => v.Avg);
                    break;
            }
            /*ViewBag.ratingsCounted = _context.ocenyTreningow.GroupBy(t => t.id_treningu)
                                                            .Select(t => new { training = t.Key, Avg = t.Average(k => k.ocena) })
                                                            .AsEnumerable()
                                                            .ToDictionary(k => k.training, v => v.Avg);*/
            isTrainer();
            return View(await trainings.Include(t => t.kategoria).Include(t => t.obrazy).Include(t => t.uzytkownik).ToListAsync());

        }

        // GET: Trening/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Message = "";
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.treningi
                .Include(t => t.kategoria)
                .Include(t => t.uzytkownik)
                .Include(t => t.obrazy)
                .FirstOrDefaultAsync(m => m.id_treningu == id);
            if (trening == null)
            {
                return NotFound();
            }

            ViewBag.trainingDetails = _context.treningSzczegoly.Where(k => k.id_treningu == id)
                                        .Include(k => k.cwiczenie)
                                        .ToList();
            var userId = int.Parse(User.Identity.GetUserId());
            Rola usersRole = _context.role.Include(k => k.uzytkownicy)
                                          .FirstOrDefault(m => m.nazwa == "trener");

            List<int> trainersIds = new List<int>();
            if (usersRole != null)
                foreach (var user in usersRole.uzytkownicy)
                {
                    trainersIds.Add(user.id_uzytkownika);
                }
            if (userId != trening.id_uzytkownika && !trainersIds.Contains(trening.id_uzytkownika))
            {
                return NotFound();
            }

            try
            {
                ViewBag.rating = _context.ocenyTreningow.Single(e => e.id_uzytkownika == userId && e.id_treningu == id);
            }
            catch
            {
                var rate = new OcenaTreningu();
                rate.ocena = 0;
                ViewBag.rating = rate;
            }

            ViewBag.trainingId = id;
            ViewBag.userId = int.Parse(this.User.Identity.GetUserId());
            ViewBag.treningOwner = trening.id_uzytkownika;

            if (trening == null)
            {
                return NotFound();
            }

            if (trening.obrazy.Count <= 0)
                ViewBag.image = null;
            else
                ViewBag.image = trening.obrazy
                                    .Last()
                                    .GetImageDataUrl();

            var link = generateYoutubeEmbededLink(trening.youtube_link);
            ViewBag.youtube = link =="" || link == null ? null : link ;
            isTrainer();
            return View(trening);
        }

        [HttpPost]
        public IActionResult Details(int id, double rating)
        {
            var training = _context.treningi
                .FirstOrDefault(m => m.id_treningu == id);

            var userId = int.Parse(this.User.Identity.GetUserId());

            if (_context.ocenyTreningow.Any(e => e.id_uzytkownika == userId && e.id_treningu == training.id_treningu))
            {
                OcenaTreningu ocena = new OcenaTreningu();
                ocena.id_uzytkownika = userId;
                ocena.id_treningu = training.id_treningu;
                ocena.ocena = rating;
                ocena.oceniajacy = _context.uzytkownicy.First(e => e.Id == userId);
                ocena.trening = training;

                _context.Update(ocena);
            }
            else
            {
                OcenaTreningu ocena = new OcenaTreningu();
                ocena.id_uzytkownika = userId;
                ocena.id_treningu = training.id_treningu;
                ocena.ocena = rating;
                ocena.oceniajacy = _context.uzytkownicy.First(e => e.Id == userId);
                ocena.trening = training;

                _context.Add(ocena);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Details));
        }

        // GET: Trening/Create
        public IActionResult Create()
        {
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa");
            isTrainer();
            return View();
        }

        // POST: Trening/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_treningu,nazwa,id_kategorii")] Trening trening)
        {
            trening.id_uzytkownika = int.Parse(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                _context.Add(trening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa", trening.id_kategorii);
            isTrainer();
            return View(trening);
        }

        // GET: Trening/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.treningi.FindAsync(id);
            if (trening == null)
            {
                return NotFound();
            }
            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            ViewData["id_kategorii"] = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa", trening.id_kategorii);

            ViewBag.exercises = await _context.treningSzczegoly.Where(t => t.id_treningu == trening.id_treningu)
                                                                .Include(t => t.cwiczenie)
                                                                .ToListAsync();
            isTrainer();
            return View(trening);
        }

        // POST: Trening/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_treningu,nazwa,id_kategorii")] Trening trening)
        {
            if (id != trening.id_treningu)
            {
                return NotFound();
            }
            Trening trainingToEdit = await _context.treningi.FindAsync(id);

            if (trainingToEdit.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            if (ModelState.IsValid)
            {
                try
                {
                    trainingToEdit.nazwa = trening.nazwa;
                    trainingToEdit.id_kategorii = trening.id_kategorii;
                    _context.Update(trainingToEdit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreningExists(trening.id_treningu))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_kategorii"] = new SelectList(_context.kategoriaTreningu, "id_kategorii", "nazwa", trening.id_kategorii);
            isTrainer();
            return View(trening);
        }

        // GET: Trening/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.treningi
                .Include(t => t.kategoria)
                .Include(t => t.uzytkownik)
                .FirstOrDefaultAsync(m => m.id_treningu == id);
            if (trening == null)
            {
                return NotFound();
            }

            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            isTrainer();
            return View(trening);
        }

        // POST: Trening/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trening = await _context.treningi.FindAsync(id);

            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            _context.treningi.Remove(trening);
            await _context.SaveChangesAsync();
            isTrainer();
            return RedirectToAction(nameof(Index));
        }

        // POST: Trening/DeleteExercise/5/6
        [HttpPost, ActionName("DeleteExercise")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExercise(int idt,int idc)
        {
            var cwiczenie = _context.treningSzczegoly.Include(k => k.trening)
                .FirstOrDefault(k => k.id_treningu == idt && k.id_cwiczenia == idc);
            if (cwiczenie == null)
                return RedirectToAction("Index");

            if (cwiczenie.trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = cwiczenie.id_treningu });

            _context.treningSzczegoly.Remove(cwiczenie);
            await _context.SaveChangesAsync();
            isTrainer();
            return RedirectToAction(nameof(Details), new { id = idt});
        }

        // GET: Trening/AddExercise/5       
        public async Task<IActionResult> AddExercise(int? id, string category)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trening = await _context.treningi.FindAsync(id);
            if (trening == null)
            {
                return NotFound();
            }
            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            TreningSzczegoly tszczegoly = new TreningSzczegoly();
            tszczegoly.id_treningu = trening.id_treningu;
            ViewBag.trainingId = id;
            
            SelectList categories = new SelectList(_context.kategoriaCwiczenia, "id_kategorii", "nazwa");
            List<SelectListItem> _categories = categories.ToList();
            _categories.Insert(0, new SelectListItem() { Value = "-1", Text = "Wszystkie" });
            ViewBag.category = new SelectList((IEnumerable<SelectListItem>)_categories, "Value", "Text");

            if (!String.IsNullOrEmpty(category) && category != "-1")
            {
                int idc = int.Parse(category);
                ViewData["id_cwiczenia"] = new SelectList(_context.cwiczenia.Where(k => k.id_kategorii == idc), "id_cwiczenia", "nazwa");
            }
            else
                ViewData["id_cwiczenia"] = new SelectList(_context.cwiczenia, "id_cwiczenia", "nazwa");

            isTrainer();
            return View(tszczegoly);
        }

        // POST: Trening/AddExercise/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExercise(int id, [Bind("id_cwiczenia,liczba_powtorzen")] TreningSzczegoly tszczegoly)
        {
            Trening trening = _context.treningi.FirstOrDefault(x => x.id_treningu == id);
            if(trening == null)
            {
                return NotFound();
            }

            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            if (ExerciseAlreadyInTraining(trening.id_treningu, tszczegoly.id_cwiczenia))
                return View("AlreadyExists");

            tszczegoly.id_treningu = id;
            if(tszczegoly.liczba_powtorzen <= 0)
            {
                tszczegoly.liczba_powtorzen = 1;
            }

            if (ModelState.IsValid)
            {
                _context.Add(tszczegoly);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddExercise), new { id = trening.id_treningu});
            }
            ViewBag.trainingId = id;
            ViewData["id_cwiczenia"] = new SelectList(_context.cwiczenia, "id_cwiczenia", "nazwa", tszczegoly.id_cwiczenia);
            isTrainer();
            return View(tszczegoly);
        }

        public IActionResult AddImage(int id)
        {
            if (!_context.treningi.Any(t => t.id_treningu == id))
            {
                ViewBag.Message = "Nie ma takiego treningu";
                ViewBag.training = false;
                return View("AddImage");
            }
            var trening = _context.treningi.FirstOrDefault(t => t.id_treningu == id);
            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            ViewBag.Message = "";
            ViewBag.training = true;
            isTrainer();
            return View();
        }

        [HttpPost, ActionName("AddImage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImagePost(int id)
        {
            if(!_context.treningi.Any(t => t.id_treningu == id))
            {
                ViewBag.Message = "Nie ma takiego treningu";
                ViewBag.training = false;
                return View("AddImage");
            }

            var trening = await _context.treningi.FirstOrDefaultAsync(t => t.id_treningu == id);
            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            ViewBag.training = false;
            //ViewBag.training = true;
            var file = Request.Form.Files.Count != 0 ? Request.Form.Files[0] : null;

            if(file == null)
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
            
            ViewBag.training = true;
            ObrazyTreningu image = new ObrazyTreningu();
            image.id_treningu = id;
            image.format = fileExtension;

            MemoryStream memeoryStream = new MemoryStream();
            file.CopyTo(memeoryStream);
            image.obraz = memeoryStream.ToArray();

            memeoryStream.Close();
            memeoryStream.Dispose();

            await _context.obrazyTreningow.AddAsync(image);
            await _context.SaveChangesAsync();
            ViewBag.Message = "Obraz został dodany";
            isTrainer();
            return RedirectToAction("Details", new { id = trening.id_treningu });
        }

        public IActionResult AddLink(int id)
        {
            if (!_context.treningi.Any(t => t.id_treningu == id))
            {
                ViewBag.Message = "Nie ma takiego treningu";
                ViewBag.training = false;
                return View("AddLink");
            }
            var trening = _context.treningi.FirstOrDefault(t => t.id_treningu == id);
            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            ViewBag.Message = "";
            ViewBag.training = true;
            isTrainer();
            return View();
        }

        [HttpPost, ActionName("AddLink")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLink(int id, string link)
        {
            if (!_context.treningi.Any(t => t.id_treningu == id))
            {
                ViewBag.Message = "Nie ma takiego treningu";
                ViewBag.training = false;
                return View("AddLink");
            }

            var trening = await _context.treningi.FirstOrDefaultAsync(t => t.id_treningu == id);
            if (trening.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Details", new { id = trening.id_treningu });

            ViewBag.training = true;
            trening.youtube_link = link;

            _context.treningi.Update(trening);
            await _context.SaveChangesAsync();
            ViewBag.Message = "Trening został zaktualizowany";
            isTrainer();
            return RedirectToAction("Details", new { id = trening.id_treningu });
        }

        private bool ExerciseAlreadyInTraining(int training_id, int exercise_id)
        {
            return _context.treningSzczegoly.Any(e => e.id_treningu == training_id && e.id_cwiczenia == exercise_id);
        }

        private bool TreningExists(int id)
        {
            return _context.treningi.Any(e => e.id_treningu == id);
        }

        private bool isTrainer()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            List<RolaUzytkownika> usersRoles = _context.RolaUzytkownika.Where(k => k.id_uzytkownika == userId).Include(c => c.rola).ToList();

            foreach (var usersRole in usersRoles)
            {
                if (usersRole.rola.nazwa == "admin")
                {
                    ViewBag.ifAdmin = true;
                    return true;
                }
                if (usersRole.rola.nazwa == "trener")
                    return true;
            }
            return false;
        }

        private double trainingRating(int traning_id)
        {
            if (!_context.ocenyTreningow.Any(k => k.id_treningu == traning_id))
                return 0;
            double ratings_avg = _context.ocenyTreningow
                                      .Where(k => k.id_treningu == traning_id)
                                      .Average(k => k.ocena);
            return ratings_avg;
        }

        public static string generateYoutubeEmbededLink(string orginalLink)
        {
            if (orginalLink == null) return "";
            string embededLink = "https://www.youtube.com/embed/{0}";
            if(orginalLink.Contains("https://youtu.be/"))
            {
                string[] seperated_link = orginalLink.Split('/');
                embededLink = string.Format(embededLink, seperated_link[seperated_link.Length - 1]);
                return embededLink;
            }
            else
            {
                if (orginalLink.Contains("watch?v="))
                {
                    int first_element_equals = orginalLink.IndexOf('=');
                    int first_element_and = orginalLink.Contains("&") ? orginalLink.IndexOf('&') : orginalLink.Length - 1;
                    embededLink = string.Format(embededLink,
                        orginalLink.Substring(first_element_equals + 1, first_element_and - first_element_equals));
                    return embededLink;
                }
                else return "";
            }
            
        }

    }
}