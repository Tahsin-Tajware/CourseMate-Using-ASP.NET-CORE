using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseMate.Models
{
  public class Vote
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }   // Identity User (string)

    [ForeignKey("UserId")]
    public virtual Users User { get; set; }

    [Required]
    public int VotableId { get; set; }   // FK for Post or Comment

    [Required]
    public string VotableType { get; set; }   // "Post" or "Comment"

    public int Value { get; set; }  // e.g. 1 = upvote, -1 = downvote
  }
}
