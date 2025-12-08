using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace CineSoul.Controllers
{
    // Sadece giriş yapmış kullanıcılar erişebilir
    [Authorize]
    public class ProfileController : Controller
    {
        // Profilim sayfasının ana giriş noktası
        // Buraya listeleri, kullanıcı bilgilerini vb. çekmek için mantık ekleyebilirsiniz.
        public IActionResult Index()
        {
            ViewData["Title"] = "Kullanıcı Profili";

            // Eğer Views/Profile/Index.cshtml dosyanız yoksa,
            // bu projeyi çalıştırmadan önce onu oluşturmanız gerekir.
            return View();
        }
    }
}