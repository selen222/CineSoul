using CineSoul.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineSoul.Controllers
{
            var userId = _userManager.GetUserId(User);

            var userLists = await _context.UserLists
                                        .Where(l => l.OwnerId == userId)
                                        .ToListAsync();

            return View(userLists);
        }

        // Tek bir listenin detaylarını (ve içindeki filmleri) gösterir
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var userList = await _context.UserLists
                                        .FirstOrDefaultAsync(l => l.Id == id && l.OwnerId == userId);

            if (userList == null)
            {
                return NotFound();
            }

            var movies = new List<Movie>();
            foreach (var tmdbId in userList.MovieIds)
            {
                var movie = await _tmdbService.GetFullMovieDetailsAsync(tmdbId);
                if (movie != null)
                {
                    movies.Add(movie);
                }

            ViewBag.Movies = movies;
            return View(userList);
        }

        // ==========================================
        // LİSTE OLUŞTURMA (CREATE)
        // ==========================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] UserList userList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userList);
                await _context.SaveChangesAsync();
        }

        // ==========================================
        // LİSTEDEN FİLM EKLEME/ÇIKARMA
        // ==========================================

        // Bu metot, genellikle bir AJAX çağrısı ile bir film detay sayfasından tetiklenir
        [HttpPost]
        public async Task<IActionResult> ToggleMovie(int listId, int tmdbMovieId)
        {
            var userId = _userManager.GetUserId(User);

            var userList = await _context.UserLists
                                        .FirstOrDefaultAsync(l => l.Id == listId && l.OwnerId == userId);

            if (userList == null)
            {
                return NotFound("Liste bulunamadı.");
            }

            if (userList.MovieIds.Contains(tmdbMovieId))
            {
                userList.MovieIds.Remove(tmdbMovieId);
                await _context.SaveChangesAsync();
                return Json(new { success = true, added = false, message = "Film listeden çıkarıldı." });
            }
            else
            {
                userList.MovieIds.Add(tmdbMovieId);
                await _context.SaveChangesAsync();
                return Json(new { success = true, added = true, message = "Film listeye eklendi." });
            }
        }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}