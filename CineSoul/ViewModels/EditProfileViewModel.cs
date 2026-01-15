namespace CineSoul.ViewModels
{
    public class EditProfileViewModel
    {
        public string? DisplayName { get; set; }

        // Fotoğraf yüklemek için IFormFile kullanıyoruz
        public IFormFile? ProfilePictureFile { get; set; }

        // Mevcut fotoğrafı göstermek için
        public string? ExistingPicturePath { get; set; }
    }
}