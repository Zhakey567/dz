using System.Security.Claims;
using dz.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dz.Pages;
[Authorize(Roles = "student")]
public class TaskModel : PageModel
{
    private readonly ApplicationDbContext _context;
     
         public TaskModel(ApplicationDbContext context)
         {
             _context = context;
         }
     
         public IList<Assignment> HomeworkList { get; set; }
     
         public async Task OnGetAsync()
         {

             var email = User.Identity.Name;
             var student = await _context.Students
                 .FirstOrDefaultAsync(s => s.Email == email);
                
             var course = await _context.Courses
                 .FirstOrDefaultAsync(s => s.StudentId == student.Id);

             
             HomeworkList = await _context.Assignments
                 .Where(h => h.ClassId == course.ClassId) 
                 .ToListAsync();
         }
}