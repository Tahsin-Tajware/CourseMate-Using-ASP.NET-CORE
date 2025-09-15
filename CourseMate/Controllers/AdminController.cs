// AdminController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CourseMate.Models;
using CourseMate.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CourseMate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public AdminController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/ReportedComments with filtering
        public async Task<IActionResult> ReportedComments(string statusFilter = "Pending", int page = 1, int pageSize = 20)
        {
            ViewBag.StatusFilter = statusFilter;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            IQueryable<ReportedComment> query = _context.ReportedComments
                .Include(rc => rc.Comment)
                    .ThenInclude(c => c.User)
                .Include(rc => rc.User)
                .Include(rc => rc.Comment)
                    .ThenInclude(c => c.Post);

            if (statusFilter != "All")
            {
                if (Enum.TryParse(statusFilter, out ReportStatus statusEnum))
                {
                    query = query.Where(rc => rc.Status == statusEnum);
                }
            }

            var totalCount = await query.CountAsync();
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var reportedComments = await query
                .OrderByDescending(rc => rc.ReportedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(reportedComments);
        }

        // POST: Admin/RemoveComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveComment(int commentId, int reportId, string returnUrl = null)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            // Soft delete the comment (moderator removal)
            comment.IsRemovedByModerator = true;
            comment.DeletedAt = DateTime.UtcNow;
            comment.DeletedByUserId = user.Id;
            _context.Comments.Update(comment);

            // Update the report status
            var report = await _context.ReportedComments.FindAsync(reportId);
            if (report != null)
            {
                report.Status = ReportStatus.Resolved;
                _context.ReportedComments.Update(report);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comment removed successfully.";
            return RedirectToAction(nameof(ReportedComments));
        }

        // POST: Admin/RejectReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectReport(int reportId, string returnUrl = null)
        {
            var report = await _context.ReportedComments.FindAsync(reportId);
            if (report == null)
            {
                return NotFound();
            }

            report.Status = ReportStatus.Rejected;
            _context.ReportedComments.Update(report);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Report rejected successfully.";
            return RedirectToAction(nameof(ReportedComments));
        }

        // POST: Admin/ApproveComment (restore if previously removed)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveComment(int commentId, int reportId, string returnUrl = null)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            // Restore the comment if it was removed
            comment.IsRemovedByModerator = false;
            comment.DeletedAt = null;
            comment.DeletedByUserId = null;
            _context.Comments.Update(comment);

            // Update the report status
            var report = await _context.ReportedComments.FindAsync(reportId);
            if (report != null)
            {
                report.Status = ReportStatus.Resolved;
                _context.ReportedComments.Update(report);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comment approved and restored successfully.";
            return RedirectToAction(nameof(ReportedComments));
        }

        // GET: Admin/DeletedComments (view all soft-deleted comments)
        public async Task<IActionResult> DeletedComments(int page = 1, int pageSize = 20)
        {
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            var deletedComments = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .Include(c => c.DeletedByUser)
                .Where(c => c.IsDeleted || c.IsRemovedByModerator)
                .OrderByDescending(c => c.DeletedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.Comments
                .CountAsync(c => c.IsDeleted || c.IsRemovedByModerator);

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return View(deletedComments);
        }

        // POST: Admin/PermanentlyDeleteComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PermanentlyDeleteComment(int commentId, string returnUrl = null)
        {
            var comment = await _context.Comments
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            // Remove replies first
            _context.Comments.RemoveRange(comment.Replies);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comment permanently deleted successfully.";
            return RedirectToAction(nameof(DeletedComments));
        }

        // POST: Admin/RestoreComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreComment(int commentId, string returnUrl = null)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            // Restore the comment
            comment.IsDeleted = false;
            comment.IsRemovedByModerator = false;
            comment.DeletedAt = null;
            comment.DeletedByUserId = null;
            _context.Comments.Update(comment);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comment restored successfully.";
            return RedirectToAction(nameof(DeletedComments));
        }
    }
}