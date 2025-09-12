namespace CourseMate.Models.ViewModels
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public string CurrentUserId { get; set; }
        public int PostId { get; set; }
        public int Depth { get; set; } = 0;
    }
}
