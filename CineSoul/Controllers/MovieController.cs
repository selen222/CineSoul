using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CineSoul.Models;
using CineSoul.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using CineSoul.Data;
using Microsoft.EntityFrameworkCore;

namespace CineSoul.Controllers
{
    public class MoviesController : Controller
    {
        // Alanları null olamaz olarak tanımlamak için ! operatörü kullanıldı, DI tarafından doldurulacağı varsayılır.
        private readonly ITmdbService _tmdbService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MoviesController(
            ITmdbService tmdbService, // Servis tipini arayüz (Interface) olarak kullanmak daha doğru
            UserManager<AppUser> userManager,
            ApplicationDbContext context)
        {
            _tmdbService = tmdbService;
            _userManager = userManager;
            _context = context;
        }

        // =======================
        // ANA SAYFA
        // =======================
        public async Task<IActionResult> Index()
        {
            var trendingMovies = await _tmdbService.GetTrendingMoviesAsync();
            var popularMovies = await _tmdbService.GetPopularMoviesAsync();
            var nowPlayingMovies = await _tmdbService.GetNowPlayingMoviesAsync();

            ViewBag.Trending = trendingMovies;
            ViewBag.Popular = popularMovies;
            ViewBag.NowPlaying = nowPlayingMovies;

            return View();
        }

        // ... Diğer Index, Trending, NowPlaying, Popular metotları aynı kalır ...

        // =======================
        // Film detay sayfası (id ile)
        // =======================
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _tmdbService.GetFullMovieDetailsAsync(id);

            // CS8602 Hatası çözümü: movie değişkeni null olabilir, bu yüzden kontrol etmek zorundayız
            if (movie == null)
            {
                return NotFound();
            }

            if (User.Identity!.IsAuthenticated) // Identity'nin null olamayacağını belirtmek için ! kullanıldı
            {
                // GetUserId null döndürmeyeceği için ! kullanıldı
                var userId = _userManager.GetUserId(User)!;

                var userLists = await _context.UserLists
                                            .Where(l => l.OwnerId == userId)
                                            .ToListAsync();

                ViewBag.UserLists = userLists;
            }

            return View(movie);
        }

        // =======================
        // Film Arama (Search)
        // =======================
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // Arama sorgusu yoksa boş bir sonuç sayfası göster
                return View(new List<Movie>());
            }

            // TmdbService kullanarak arama yap
            var searchResults = await _tmdbService.SearchMoviesAsync(query);

            // Arama terimini View'a başlık olarak gönder
            ViewBag.SearchQuery = query;

            // Sonuçları View'a gönder
            return View(searchResults);
        }
    }
}