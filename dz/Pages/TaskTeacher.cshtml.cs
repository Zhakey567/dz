using System.Security.Claims;
using dz.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dz.Pages;

[Authorize(Roles = "teacher")]
public class TaskTeacherModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public TaskTeacherModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] 
    public IList<Assignment> HomeworkList { get; set; }

    public List<int> Cour { get; set; }

     

    public async Task OnGetAsync()
    {
        Cour = new List<int>();
        var email = User.Identity.Name;
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Email == email);

        var classes = await _context.Courses
            .Where(h => h.TeacherId == student.Id)
            .ToListAsync();

        foreach (var cla in classes)
        {
            Cour.Add(cla.ClassId);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var selectedClassId = int.Parse(Request.Form["class"]);
        
        HomeworkList = await _context.Assignments
            .Where(h => h.ClassId == selectedClassId)
            .ToListAsync();
        
        
        Cour = new List<int>();
        var email = User.Identity.Name;
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Email == email);

        var classes = await _context.Courses
            .Where(h => h.TeacherId == student.Id)
            .ToListAsync();

        foreach (var cla in classes)
        {
            Cour.Add(cla.ClassId);
        }

        return Page();
    }
}