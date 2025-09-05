using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseMate.Models
{
  public class Comment
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual Users User { get; set; }

    [Required]
    public int PostId { get; set; }

    [ForeignKey("PostId")]
    public virtual Post Post { get; set; }

    public int? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public virtual Comment Parent { get; set; }

    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

    public bool IsAnonymous { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
