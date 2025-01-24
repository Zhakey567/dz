using dz.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace dz.Pages;

[Authorize(Roles = "teacher")]
[IgnoreAntiforgeryToken]
public class CreateModel : PageModel
{
    ApplicationDbContext context;
    [BindProperty]
    public Assignment Assignment1 { get; set; } = new();
    public CreateModel(ApplicationDbContext db)
    {
        context = db;
    }
    public List<int> Cour { get; set; }

    public async Task OnGetAsync()
    {
        Cour = new List<int>();
        var email = User.Identity.Name;
        var student = await context.Students
            .FirstOrDefaultAsync(s => s.Email == email);

        var classes = await context.Courses
            .Where(h => h.TeacherId == student.Id)
            .ToListAsync();

        foreach (var cla in classes)
        {
            Cour.Add(cla.ClassId);
        }
    }
    public async Task<IActionResult> OnPostAsync()
    {
        Assignment1.Datetime = DateTime.UtcNow;
        var classId = int.Parse(Request.Form["classId"]);
        var subject = Request.Form["subject"];
        var message = Request.Form["message"];
        var newAssignments = new Assignment();
        newAssignments.ClassId = classId;
        newAssignments.Subject = subject;
        newAssignments.Message = message;
        context.Assignments.Add(newAssignments);
        await context.SaveChangesAsync();


        var studentsId = context.Courses
            .Where(h => h.ClassId == classId).ToList();
        var stId = new List<int>();
        foreach (var st in studentsId)
        {
            stId.Add(st.StudentId);
        }
        
        var emails = await context.Students
            .Where(student => stId.Contains(student.Id)) 
            .Select(student => student.Email) 
            .ToListAsync(); 
        foreach (var st in emails)
        {
            SendEmailAsync(st, subject, message);
        }
      
        return RedirectToPage("Index");
    }
    
    
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        using var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Задание для практики", "zhakeevtimur567@yandex.ru"));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message
        };
        email.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync("smtp.yandex.ru", 465, true);
            await smtp.AuthenticateAsync("zhakeevtimur567@yandex.ru","zedkxawbkoqtfpwq");
 
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            
            throw;
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}