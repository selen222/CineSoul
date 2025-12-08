using System.Diagnostics;
using CineSoul.Models;
using CineSoul.Services;          // TMDB Servis Arayüzü için eklendi
using CineSoul.ViewModels;       // HomeViewModel için eklendi
using Microsoft.AspNetCore.Mvc;

namespace CineSoul.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITmdbService _tmdbService; // TMDB servisiniz buraya enjekte ediliyor

        // Constructor güncellendi: Hem ILogger hem de ITmdbService almalý
        public HomeController(ILogger<HomeController> logger, ITmdbService tmdbService)
        {
            _logger = logger;
            _tmdbService = tmdbService;
        }

        // Index metodu, API'dan asenkron (async) veri çekmek için güncellendi
        public async Task<IActionResult> Index()
        {
            try
            {
                // API servisinizden gerekli tüm listeleri çekin
                // NOT: ITmdbService'de bu metotlarýn var olduðunu varsayýyoruz.
                // Eðer yoksa, bu metotlarý ITmdbService'e eklemeniz gerekir.
                var viewModel = new HomeViewModel
                {
                    // Hero Carousel için (Örn: Vizyondakiler)
                    HeroCarouselMovies = await _tmdbService.GetNowPlayingMoviesAsync(),
                    
                    // Yatay Listeler için
                    TrendingMovies = await _tmdbService.GetTrendingMoviesAsync(),
                    NewReleases = await _tmdbService.GetNewReleasesAsync(),
                    ComingSoon = await _tmdbService.GetUpcomingMoviesAsync()
                };

                return View(viewModel); // ViewModel'i View'a gönder
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yap ve boþ bir model gönder
                _logger.LogError(ex, "Ana sayfa yüklenirken TMDB API'dan veri çekme hatasý oluþtu.");
                
                // Kullanýcýya boþ veya kýsmi bir sayfa göstermek için boþ bir model döndür.
                return View(new HomeViewModel()); 
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}