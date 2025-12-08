using CineSoul.Data;
using CineSoul.Models;
using CineSoul.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineSoul.Controllers
{
    // Giriş yapmamış kullanıcıların erişimini engeller
    [Authorize]
    public class UserListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITmdbService _tmdbService;

        public UserListController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ITmdbService tmdbService)
        {
            _context = context;
            _userManager = userManager;
            _tmdbService = tmdbService;
        }

        // ==========================================
        // LİSTE GÖRÜNTÜLEME (INDEX/DETAY)
        // ==========================================

        // Kullanıcının TÜM listelerini gösterir
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            // Kullanıcının tüm listelerini çeker
            var userLists = await _context.UserLists
                                        .Where(l => l.OwnerId == userId)
                                        .ToListAsync();

            return View(userLists);
        }

        // Tek bir listenin detaylarını (ve içindeki filmleri) gösterir
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            // Kullanıcının listeyi gerçekten sahiplendiğini kontrol et
            var userList = await _context.UserLists
                                        .FirstOrDefaultAsync(l => l.Id == id && l.OwnerId == userId);

            if (userList == null)
            {
                return NotFound();
            }

            // Listede yer alan film ID'lerini kullanarak detayları çek
            // Not: TmdbService'de toplu film çekme metodu yoksa, tek tek çekmek zorunda kalırız.
            var movies = new List<Movie>();
            foreach (var tmdbId in userList.MovieIds)
            {
                // API'dan film detayını çek
                // TmdbService'de GetBaseMovieDetailsAsync metodunuz olduğunu varsayıyorum
                var movie = await _tmdbService.GetFullMovieDetailsAsync(tmdbId);
                if (movie != null)
                {
                    movies.Add(movie);
                }
            }

            // Filmleri View'a gönder
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
                userList.OwnerId = _userManager.GetUserId(User);
                userList.Type = ListType.Custom; // Varsayılan olarak özel liste

                _context.Add(userList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userList);
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

            // Filmin listede olup olmadığını kontrol et
            if (userList.MovieIds.Contains(tmdbMovieId))
            {
                // Zaten varsa, listeden çıkar
                userList.MovieIds.Remove(tmdbMovieId);
                await _context.SaveChangesAsync();
                return Json(new { success = true, added = false, message = "Film listeden çıkarıldı." });
            }
            else
            {
                // Yoksa, listeye ekle
                userList.MovieIds.Add(tmdbMovieId);
                await _context.SaveChangesAsync();
                return Json(new { success = true, added = true, message = "Film listeye eklendi." });
            }
        }

        // ==========================================
        // LİSTE SİLME (DELETE)
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var userList = await _context.UserLists
                                        .FirstOrDefaultAsync(l => l.Id == id && l.OwnerId == userId);

            if (userList != null)
            {
                _context.UserLists.Remove(userList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound(); // Liste bulunamazsa 404 dön
        }
    }
}