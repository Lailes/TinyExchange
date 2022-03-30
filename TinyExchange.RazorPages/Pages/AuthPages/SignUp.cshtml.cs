using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyExchange.RazorPages.Database.Managers.Auth;

namespace TinyExchange.RazorPages.Pages.AuthPages;

public class SignUp : PageModel
{
    public string? Message { get; private set; } 

    public async Task OnGet([FromServices] IAuthManager authManager, string? message = null)
    {
        Message = message;
        if (User.Identity?.IsAuthenticated ?? false) await authManager.SignOutAsync(HttpContext);
    }

    public async Task<ActionResult> OnPost([FromServices] IAuthManager authManager, string firstName, string lastName, string email, string password) =>
        await authManager.SignUpAsync(new (firstName, lastName, email, password), HttpContext) switch {
            OkSignUpResult ok => RedirectToPage($"KycRequest", "Message", new { message = "You need to proceed KYC" }),
            EmailAlreadyRegisteredResult => RedirectToPage("../AuthPages/SignUp", new {message = "Email already registered"}),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
}