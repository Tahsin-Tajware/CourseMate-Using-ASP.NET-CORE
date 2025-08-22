using Microsoft.AspNetCore.Identity;

namespace CourseMate.Models
{
    public class Users : IdentityUser
    {
        public string FullName{ get; set; }
    }
}
