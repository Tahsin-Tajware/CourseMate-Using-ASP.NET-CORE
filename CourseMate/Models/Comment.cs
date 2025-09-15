using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        // Soft delete properties
        public bool IsDeleted { get; set; } = false;
        public bool IsRemovedByModerator { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string DeletedByUserId { get; set; }
        
        [ForeignKey("DeletedByUserId")]
        public virtual Users DeletedByUser { get; set; }
        
        // Helper properties for vote counts
        [NotMapped]
        public int Upvotes => Votes?.Count(v => v.Value == 1) ?? 0;

        [NotMapped]
        public int Downvotes => Votes?.Count(v => v.Value == -1) ?? 0;

        // Helper to check if current user has voted
        [NotMapped]
        public int CurrentUserVote { get; set; }
    }
}