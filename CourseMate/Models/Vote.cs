using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseMate.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // FK for the user who voted

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        // FK for the post being voted on (nullable if vote is on a comment)
        public int? PostId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        // FK for the comment being voted on (nullable if vote is on a post)
        public int? CommentId { get; set; }

        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }

        [Required]
        public int Value { get; set; }  // 1 = upvote, -1 = downvote

        [NotMapped]
        public bool IsUpvote => Value == 1;

        [NotMapped]
        public bool IsDownvote => Value == -1;
    }
}
