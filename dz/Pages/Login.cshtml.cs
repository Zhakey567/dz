using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using dz.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dz.Pages;

public class LoginModel : PageModel
{
    [BindProperty] public InputModel Input { get; set; }
    ApplicationDbContext context;
    public Assignment Assignment1 { get; set; } = new();

    public LoginModel(ApplicationDbContext db)
    {
        context = db;
    }

    public void OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            RedirectToPage("/Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Input.Email) || string.IsNullOrEmpty(Input.Password))
        {
            ModelState.AddModelError("", "Email and password are required");
            return Page();
        }

     
        var student = await context.Students
            .FirstOrDefaultAsync(p => p.Email == Input.Email);

        var user = student;
    
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid login attempt");
            return Page();
        }

    
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(24)
            });

        return RedirectToPage("/Index");
    }

    public string PrintTime() => DateTime.Now.ToShortTimeString();
}

public class InputModel
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}