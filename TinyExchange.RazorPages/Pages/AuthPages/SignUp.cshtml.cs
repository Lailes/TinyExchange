using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Pages.AuthPages;

[IgnoreAntiforgeryToken(Order = 1001)]
public class SignUp : PageModel
{
    public string? Message { get; private set; } 

    public async Task OnGet([FromServices] IAuthManager authManager, string? message = null)
    {
        Message = message;
        if (User.Identity?.IsAuthenticated ?? false) 
            await authManager.SignOutAsync(HttpContext);
    }

    public async Task<ActionResult> OnPost([FromServices] IAuthManager authManager, [FromForm] SignUpData signUpData) =>
        await authManager.SignUpAsync(signUpData, HttpContext) switch {
            OkSignUpResult => RedirectToPage($"KycRequest", "Message", new { message = "You need to proceed KYC" }),
            EmailAlreadyRegisteredResult => RedirectToPage("../AuthPages/SignUp", new {message = "Email already registered"}),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
}