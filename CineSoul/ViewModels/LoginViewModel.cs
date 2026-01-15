using System.ComponentModel.DataAnnotations;

namespace CineSoul.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-Posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir E-Posta adresi giriniz.")]
        [Display(Name = "E-Posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; } // Boş olabilir
    }
}