using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Infrastructure.Extensions;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Pages.AuthPages;

[Authorize(Roles = SystemRoles.User)]
public class KycRequest : PageModel
{
    private readonly IUserManager _userManager;
    protected readonly IKycManager _kycManager;

    public User? RequesterUser { get; set; }
    public string? Message { get; set; }

    public KycRequest(IUserManager userManager, IKycManager kycManager)
    {
        _userManager = userManager;
        _kycManager = kycManager;
    }

    public async Task OnGet() => 
        RequesterUser = await _userManager.FindUserByIdAsync(User.GetUserId());

    public async Task OnGetMessage(string message)
    {
        Message = message;
        await OnGet();
    }
    
    public async Task<IActionResult> OnPost([FromForm] KycUserRequest request)
    {
        await _kycManager.AddKycRequestInQueueAsync(request, User.GetUserId());
        return RedirectToPage("Login", new { message = "KYC Request is created" });
    }

    public bool RequestIsNeeded =>
        RequesterUser != null &&
        (RequesterUser.KycRequest == null || RequesterUser.KycRequest.KycState == KycState.Rejected);
}