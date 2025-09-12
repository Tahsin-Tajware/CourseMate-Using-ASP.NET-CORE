using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseMate.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // Foreign key for the user who voted

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        [Required]
        public int VotableId { get; set; }   // FK for Post or Comment (can be Post.Id or Comment.Id)

        [Required]
        public string VotableType { get; set; }   // Can be "Post" or "Comment"

        public int Value { get; set; }  // e.g. 1 = upvote, -1 = downvote

        [NotMapped]
        public bool IsUpvote => Value == 1;  // Helper property to check if it's an upvote

        [NotMapped]
        public bool IsDownvote => Value == -1;  // Helper property to check if it's a downvote

        public int CommentId { get; internal set; }
    }
}
