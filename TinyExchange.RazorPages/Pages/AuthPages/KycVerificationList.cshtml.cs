using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Infrastructure.Extensions;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Pages.AuthPages;

[Authorize(Roles = $"{SystemRoles.Admin},{SystemRoles.KycManager}", Policy = KycClaimSettings.PolicyName)]
public class KycVerificationList : PageModel
{
    private readonly IKycManager _kycManager;
    private readonly IUserManager _userManager;

    public IEnumerable<User> UserWithKycRequests { get; set; } = Enumerable.Empty<User>();
    public User? ViewerUser { get; set; }
    
    public KycVerificationList(IKycManager kycManager, IUserManager userManager)
    {
        _kycManager = kycManager;
        _userManager = userManager;
    }
    
    public async Task OnGet()
    {
        UserWithKycRequests = await _kycManager.QueryUsersWithRequests(kycStates: new [] { KycState.InQueue }).ToListAsync();
        ViewerUser = await _userManager.FindUserByIdAsync(User.GetUserIdFromClaims());
    }
}