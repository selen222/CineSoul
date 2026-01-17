namespace CineSoul.ViewModels
{
    public class EditProfileViewModel
    {
        public string? DisplayName { get; set; }

        public IFormFile? ProfilePictureFile { get; set; }

        public string? ExistingPicturePath { get; set; }
    }
}