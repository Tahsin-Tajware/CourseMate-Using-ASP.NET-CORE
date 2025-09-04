using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseMate.ViewModels
{
  public class CreatePostViewModel
  {
    [Required(ErrorMessage = "Course name is required")]
    [Display(Name = "Course Name")]
    public string CourseName { get; set; }

    [Required(ErrorMessage = "Course code is required")]
    [Display(Name = "Course Code")]
    public string CourseCode { get; set; }

    [Required(ErrorMessage = "University name is required")]
    [Display(Name = "University/Varsity Name")]
    public string Varsity { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Display(Name = "Content")]
    public string Content { get; set; }

    [Display(Name = "Post Anonymously")]
    public bool IsAnonymous { get; set; }

    // Additional tags beyond the course tag
    // public List<string> AdditionalTags { get; set; } = new List<string>();
  }
}
