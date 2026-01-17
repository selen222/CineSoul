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
    [Authorize]
    [Route("[controller]/[action]")]
    public class UserListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITmdbService _tmdbService;

        public UserListController(ApplicationDbContext context, UserManager<AppUser> userManager, ITmdbService tmdbService)
        {
            _context = context;
            _userManager = userManager;
            _tmdbService = tmdbService;
        }

        [HttpPost]
        [Route("ToggleMovieJson")]
        public async Task<IActionResult> ToggleMovieJson([FromBody] ToggleMovieRequest request)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Json(new { success = false, message = "Giriş yapmalısınız." });

            var userList = await _context.UserLists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.OwnerId == userId && (request.ListId == null ? l.Type == ListType.Watchlist : l.Id == request.ListId));

            if (userList == null)
            {
                userList = new UserList { Name = "İzleme Listem", OwnerId = userId, Type = ListType.Watchlist };
                _context.UserLists.Add(userList);
                await _context.SaveChangesAsync();
            }

            var existingItem = userList.Items.FirstOrDefault(i => i.MovieId == request.TmdbMovieId);
            bool added;

            if (existingItem == null)
            {
                var movieDetail = await _tmdbService.GetFullMovieDetailsAsync(request.TmdbMovieId);
                var newItem = new UserListItem
                {
                    UserListId = userList.Id,
                    MovieId = request.TmdbMovieId,
                    MovieTitle = movieDetail?.Title ?? "Bilinmeyen Film",
                    PosterPath = movieDetail?.PosterPath,
                    AddedAt = DateTime.Now
                };
                _context.UserListItems.Add(newItem);
                added = true;
            }
            else
            {
                _context.UserListItems.Remove(existingItem);
                added = false;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, added = added });
        }
    }

    public class ToggleMovieRequest
    {
        public int TmdbMovieId { get; set; }
        public int? ListId { get; set; }
    }
}