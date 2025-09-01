using Microsoft.AspNetCore.Http;

namespace CourseMate.ViewModels
{
    public class EditProfileViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; } 
        public IFormFile? ProfileImageFile { get; set; } 
    }
}
