using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseMate.Models
{
  public enum ReportStatus
  {
    Pending,
    Reviewed,
    Resolved,
    Rejected
  }

  public class ReportedComment
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Comment")]
    public int CommentId { get; set; }
    public virtual Comment Comment { get; set; }

    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; }
    public virtual Users User { get; set; }

    [MaxLength(500)]
    public string Reason { get; set; }   // why the comment was reported

    public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

    public ReportStatus Status { get; set; } = ReportStatus.Pending;
  }
}
