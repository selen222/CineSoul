using CineSoul.Data;
using Microsoft.EntityFrameworkCore;

namespace CineSoul.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(
            ITmdbService tmdbService, // Servis tipini arayüz (Interface) olarak kullanmak daha doğru
            UserManager<AppUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _tmdbService.GetFullMovieDetailsAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            if (User.Identity!.IsAuthenticated) // Identity'nin null olamayacağını belirtmek için ! kullanıldı
            {
                // GetUserId null döndürmeyeceği için ! kullanıldı
                var userId = _userManager.GetUserId(User)!;


                ViewBag.UserLists = userLists;
            }

            return View(movie);
        }
    }
}