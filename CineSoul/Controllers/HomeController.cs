using System.Diagnostics;
using CineSoul.Models;
using CineSoul.Services;
using CineSoul.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CineSoul.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITmdbService _tmdbService;

        public HomeController(ILogger<HomeController> logger, ITmdbService tmdbService)
        {
            _logger = logger;
            _tmdbService = tmdbService;
        }

        [HttpGet]
        // URL: /Home/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new HomeViewModel
                {
                    HeroCarouselMovies = await _tmdbService.GetNowPlayingMoviesAsync(),
                    TrendingMovies = await _tmdbService.GetTrendingMoviesAsync(),
                    NewReleases = await _tmdbService.GetNewReleasesAsync(),
                    ComingSoon = await _tmdbService.GetUpcomingMoviesAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ana sayfa yüklenirken TMDB API'dan veri çekme hatasý oluþtu.");
                return View(new HomeViewModel());
            }
        }

        [HttpGet]
        // URL: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}