using dz.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dz.Pages;

public class IndexModel : PageModel
{
    ApplicationDbContext context;
    private readonly ILogger<IndexModel> _logger;
    public List<Student> Students { get; private set; } = new();
    
    public IndexModel(ApplicationDbContext db)
    {
        context = db;
    }

    public void OnGet()
    {
        Students = context.Students.ToList();
    }
}