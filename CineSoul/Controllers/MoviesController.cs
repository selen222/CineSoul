using CineSoul.Data;
using CineSoul.Models;
using CineSoul.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineSoul.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITmdbService _tmdbService;
        private readonly UserManager<AppUser> _userManager;

        public MoviesController(ApplicationDbContext context, ITmdbService tmdbService, UserManager<AppUser> userManager)
        {
            _context = context;
            _tmdbService = tmdbService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Trending = await _tmdbService.GetTrendingMoviesAsync();
            ViewBag.Popular = await _tmdbService.GetPopularMoviesAsync();
            ViewBag.NowPlaying = await _tmdbService.GetNowPlayingMoviesAsync();
            return View();
        }

        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query)) return RedirectToAction(nameof(Index));

            var searchResults = await _tmdbService.SearchMoviesAsync(query);

            ViewBag.Query = query;

            return View("Search", searchResults.ToList());
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                movie = await _tmdbService.GetFullMovieDetailsAsync(id);
                if (movie != null)
                {
                    movie.BackdropPath ??= "";
                    movie.PosterPath ??= "";
                    movie.Overview ??= "Açıklama bulunmuyor.";

                    _context.Movies.Add(movie);
                    await _context.SaveChangesAsync();
                }
            }

            if (movie == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            if (userId != null)
            {
                ViewBag.UserLists = await _context.UserLists
                    .Include(l => l.Items)
                    .Where(l => l.OwnerId == userId)
                    .ToListAsync();

                var rating = await _context.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == id);
                ViewBag.UserRating = rating?.Value;

                var alreadyExists = await _context.WatchHistories
                    .AnyAsync(w => w.UserId == userId && w.MovieId == id);

                if (!alreadyExists)
                {
                    var history = new WatchHistory
                    {
                        UserId = userId,
                        MovieId = id,
                        MovieTitle = movie.Title ?? "Bilinmeyen Film",
                        PosterPath = movie.PosterPath ?? "",
                        WatchedAt = DateTime.Now
                    };
                    _context.WatchHistories.Add(history);
                    await _context.SaveChangesAsync();
                }
            }

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> AddToList(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var userId = user.Id;

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound();

            var userList = await _context.UserLists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.OwnerId == userId && l.Type == ListType.Watchlist);

            if (userList == null)
            {
                userList = new UserList
                {
                    OwnerId = userId,
                    Name = "İzleme Listem",
                    Type = ListType.Watchlist,
                    Description = "Kullanıcının varsayılan izleme listesi"
                };
                _context.UserLists.Add(userList);
                await _context.SaveChangesAsync();
            }

            var existingItem = userList.Items.FirstOrDefault(i => i.MovieId == movieId);
            if (existingItem != null)
            {
                _context.UserListItems.Remove(existingItem);
            }
            else
            {
                _context.UserListItems.Add(new UserListItem
                {
                    UserListId = userList.Id,
                    MovieId = movieId,
                    MovieTitle = movie.Title ?? "Bilinmeyen Film",
                    PosterPath = movie.PosterPath ?? "",
                    AddedAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RateMovie(int movieId, int rating)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound();

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);

            if (existingRating != null)
            {
                existingRating.Value = rating;
                existingRating.RatedAt = DateTime.Now;
                existingRating.MovieTitle = movie.Title ?? "Bilinmeyen Film";
                existingRating.PosterPath = movie.PosterPath ?? "";

                _context.Ratings.Update(existingRating);
            }
            else
            {
                var newRating = new Rating
                {
                    UserId = userId,
                    MovieId = movieId,
                    Value = rating,
                    RatedAt = DateTime.Now,
                    MovieTitle = movie.Title ?? "Bilinmeyen Film",
                    PosterPath = movie.PosterPath ?? ""
                };
                _context.Ratings.Add(newRating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = movieId });
        }
    }
}