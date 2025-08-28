using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CourseMate.Models;

namespace CourseMate.Data
{
  public class AppDbContext : IdentityDbContext<Users>
  {
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Vote> Votes { get; set; }
  }
}
