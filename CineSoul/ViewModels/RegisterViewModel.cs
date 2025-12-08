using System.ComponentModel.DataAnnotations;

namespace CineSoul.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "E-Posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir E-Posta adresi giriniz.")]
        [Display(Name = "E-Posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrarı")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        // Hata veren FullName yerine AppUser'daki DisplayName kullanılıyor
        [Required(ErrorMessage = "Görünen ad gereklidir.")]
        [Display(Name = "Görünen Ad")]
        [MaxLength(150, ErrorMessage = "Görünen ad 150 karakterden uzun olamaz.")]
        public string DisplayName { get; set; }
    }
}