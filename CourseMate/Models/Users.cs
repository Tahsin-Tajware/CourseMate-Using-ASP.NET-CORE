using Microsoft.AspNetCore.Identity;

namespace CourseMate.Models
{
  public class Users : IdentityUser
  {
    public string FullName { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUrl { get; set; }

    public virtual ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();

  }
}
