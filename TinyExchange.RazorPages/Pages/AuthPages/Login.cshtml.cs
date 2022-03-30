using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Pages.AuthPages;

public class Login : PageModel
{
    public string? Message { get; private set; }
    
    public async Task OnGet([FromServices] IAuthManager authManager, string? message = null)
    {
        Message = message;
        if (User.Identity?.IsAuthenticated ?? false)
            await authManager.SignOutAsync(HttpContext);
    }

    public async Task<ActionResult> OnPost([FromServices] IAuthManager authManager, string email, string password) =>
        await authManager.LoginAsync(new LoginData(email, password), HttpContext) switch
        {
            OkLoginResult ok    => RedirectToPage($@"../ProfilePages/{ok.User.Role}Profile", "SelfProfile"),
            WrongLoginResult    => RedirectToPage("Login", new { message = "Wrong Login Or Password" }),
            BannedResult        => RedirectToPage("Login", new { message = "Banned" }),
            KycIsRejectedResult => RedirectToPage("KystrincRequest", "Message",new { message = "KYC is Rejected for this user" }),
            KycNotCreatedResult => RedirectToPage("KycRequest"),
            KycIsInQueueResult  => RedirectToPage("Login", new { message = "KYC is in queue, please wait"}), 
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
}