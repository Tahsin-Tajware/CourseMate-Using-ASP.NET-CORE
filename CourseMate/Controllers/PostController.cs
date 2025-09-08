using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CourseMate.Models;
using CourseMate.ViewModels;
using CourseMate.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CourseMate.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public PostController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            return View(new CreatePostViewModel());
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                // Create the post
                var post = new Post
                {
                    Title = model.Title,
                    Content = model.Content ?? string.Empty,
                    IsAnonymous = model.IsAnonymous,
                    UserId = user.Id,
                    User = user
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                // Handle the course tag
                var courseTag = await _context.Tags
                    .FirstOrDefaultAsync(t => t.CourseName == model.CourseName &&
                                              t.CourseCode == model.CourseCode &&
                                              t.Varsity == model.Varsity);

                if (courseTag == null)
                {
                    // Create new course tag
                    courseTag = new Tag
                    {
                        CourseName = model.CourseName,
                        CourseCode = model.CourseCode,
                        Varsity = model.Varsity
                    };
                    _context.Tags.Add(courseTag);
                    await _context.SaveChangesAsync();
                }

                // Associate course tag with post
                post.Tags.Add(courseTag);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Post created successfully! Awaiting admin approval.";

                // Redirect back to the Create page to show the success modal
                return RedirectToAction("Create");
            }

            return View(model);
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .Include(p => p.Votes)
                .Where(p => p.Status == PostStatus.accepted)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Index
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .Include(p => p.Votes)
                .Where(p => p.Status == PostStatus.accepted)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(posts);
        }

        [Authorize]
        public async Task<IActionResult> MyPosts()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .Include(p => p.Votes)
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            if (!posts.Any())
            {
                TempData["InfoMessage"] = "No posts found for the current user.";
            }

            return View(posts);
        }

        // GET: Post/GetCourseInfo (AJAX endpoint for autocomplete)
        [HttpGet]
        public async Task<IActionResult> GetCourseInfo(string term)
        {
            var courses = await _context.Tags
                .Where(t => !string.IsNullOrEmpty(t.CourseCode) &&
                           (t.CourseName.Contains(term) || t.CourseCode.Contains(term)))
                .Select(t => new
                {
                    label = $"{t.CourseName} ({t.CourseCode}) - {t.Varsity}",
                    courseName = t.CourseName,
                    courseCode = t.CourseCode,
                    varsity = t.Varsity
                })
                .Distinct()
                .Take(10)
                .ToListAsync();

            return Json(courses);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PendingPosts()
        {
            var pendingPosts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .Include(p => p.Votes)
                .Where(p => p.Status == PostStatus.pending)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(pendingPosts);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, PostStatus status)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Status = status;
            _context.Update(post);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Post status updated to {status}.";
            return RedirectToAction(nameof(PendingPosts));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Post deleted successfully.";
            return RedirectToAction(nameof(PendingPosts));
        }
    }
}
