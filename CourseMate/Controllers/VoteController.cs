using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CourseMate.Models;
using CourseMate.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CourseMate.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public VoteController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(int id, string votableType, int value)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Validate vote value
            if (value != 1 && value != -1)
                return BadRequest("Invalid vote value");

            if (votableType == "Post")
            {
                var post = await _context.Posts
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == id);
                
                if (post == null) return NotFound();
                
                // Check if user is trying to vote on their own post
                if (post.UserId == user.Id)
                {
                    TempData["ErrorMessage"] = "You cannot vote on your own post";
                    return RedirectToAction("Details", "Post", new { id });
                }

                await ProcessVote(post.Id, null, user.Id, value);
                return RedirectToAction("Details", "Post", new { id });
            }
            else if (votableType == "Comment")
            {
                var comment = await _context.Comments
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);
                
                if (comment == null) return NotFound();
                
                // Check if user is trying to vote on their own comment
                if (comment.UserId == user.Id)
                {
                    TempData["ErrorMessage"] = "You cannot vote on your own comment";
                    return RedirectToAction("Details", "Post", new { id = comment.PostId });
                }

                await ProcessVote(null, comment.Id, user.Id, value);
                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }

            return BadRequest("Invalid votable type");
        }

        private async Task ProcessVote(int? postId, int? commentId, string userId, int value)
        {
            // Check if user already voted
            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => 
                    v.UserId == userId && 
                    v.PostId == postId && 
                    v.CommentId == commentId);

            if (existingVote != null)
            {
                if (existingVote.Value == value)
                {
                    // Remove vote if clicking same vote type again
                    _context.Votes.Remove(existingVote);
                }
                else
                {
                    // Update vote if changing vote type
                    existingVote.Value = value;
                    _context.Votes.Update(existingVote);
                }
            }
            else
            {
                // Create new vote
                var vote = new Vote
                {
                    UserId = userId,
                    PostId = postId,
                    CommentId = commentId,
                    Value = value
                };
                _context.Votes.Add(vote);
            }

            await _context.SaveChangesAsync();
        }
    }
}