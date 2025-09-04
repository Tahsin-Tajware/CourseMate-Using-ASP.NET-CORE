using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CourseMate.Models;
using CourseMate.Data;

namespace CourseMate.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
      _logger = logger;
      _context = context;
    }

    // GET: Home
    public async Task<IActionResult> Index()
    {
      var posts = await _context.Posts
          .Include(p => p.User)
          .Include(p => p.Tags)
          .Include(p => p.Comments)
          .Include(p => p.Votes)
          .OrderByDescending(p => p.Id)
          .ToListAsync();

      return View(posts);
    }

    [Authorize]
    public IActionResult Privacy() => View();

    [Authorize(Roles = "Admin")]
    public IActionResult Admin() => View();

    [Authorize(Roles = "User")]
    public IActionResult User() => View();

    [Authorize]
    public IActionResult AskQuestion() => View();

    [Authorize]
    public IActionResult MyPosts() => View();

    [Authorize]
    public IActionResult SavedQuestions() => View();

    [Authorize]
    public IActionResult AllTags() => View();

    public IActionResult About() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
