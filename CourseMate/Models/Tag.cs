using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseMate.Models
{
  public class Tag
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string CourseName { get; set; }

    [Required]
    public string CourseCode { get; set; }

    [Required]
    public string Varsity { get; set; }

    // Many-to-Many with Posts
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
  }
}
