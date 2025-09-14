using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseMate.Models
{
  public class SavedPost
  {
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }
    public virtual Users User { get; set; }

    [ForeignKey("Post")]
    public int PostId { get; set; }
    public virtual Post Post { get; set; }

    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
  }
}
