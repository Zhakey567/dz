using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dz.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<Class> Classs { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=s1;Username=postgres;Password=123123;");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}