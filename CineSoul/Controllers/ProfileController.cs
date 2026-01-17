using CineSoul.Data;
using CineSoul.Models;
using CineSoul.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CineSoul.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var recentMovies = await _context.WatchHistories
                .Include(h => h.Movie)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.WatchedAt)
                .Take(5)
                .Select(h => h.Movie)
                .ToListAsync();

            var watchlistIds = await _context.UserListItems
                .Where(li => li.UserList.OwnerId == userId)
                .Select(li => li.MovieId)
                .ToListAsync();

            var profileViewModel = new CineSoul.ViewModels.ProfileViewModel
            {
                RecentMovies = recentMovies,
                Watchlist = watchlistIds
            };

            var userList = await _context.UserLists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.OwnerId == userId && l.Type == ListType.Watchlist);

            if (userList == null)
            {
                userList = new UserList { Items = new List<UserListItem>() };
            }

            ViewBag.UserRatings = await _context.Ratings
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RatedAt)
                .ToListAsync();

            ViewData["ProfileInfo"] = profileViewModel;

            return View(userList);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromList(int movieId)
        {
            var userId = _userManager.GetUserId(User);
            var itemToRemove = await _context.UserListItems
                .Include(i => i.UserList)
                .FirstOrDefaultAsync(i => i.MovieId == movieId && i.UserList.OwnerId == userId);

            if (itemToRemove != null)
            {
                _context.UserListItems.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new EditProfileViewModel
            {
                DisplayName = user.DisplayName,
                ExistingPicturePath = user.ProfilePicturePath
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                user.DisplayName = model.DisplayName;
                if (model.ProfilePictureFile != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePictureFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePictureFile.CopyToAsync(fileStream);
                    }

                    user.ProfilePicturePath = "/uploads/profiles/" + uniqueFileName;
                }

                await _userManager.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }

            model.ExistingPicturePath = user.ProfilePicturePath;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProfilePicture()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            if (!string.IsNullOrEmpty(user.ProfilePicturePath) && !user.ProfilePicturePath.Contains("default"))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePicturePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            user.ProfilePicturePath = null;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu." });
        }
    }
}