using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseMate.Models
{
  public class Post
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    public string Content { get; set; }

    public bool IsAnonymous { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }

    public virtual Users User { get; set; }
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

  }
}
