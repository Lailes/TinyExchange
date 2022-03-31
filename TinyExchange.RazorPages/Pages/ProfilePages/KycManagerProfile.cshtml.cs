using Microsoft.AspNetCore.Mvc;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;

namespace TinyExchange.RazorPages.Pages.ProfilePages;

public class KycManagerProfile : ProfilePage
{
    public KycManagerProfile(IUserManager userManager, IBlockingManager blockingManager, IAuthManager authManager, IAmountManager amountManager) : 
        base(userManager, blockingManager, authManager, amountManager) { }

    
    public async Task<IActionResult> OnPostChangeKycState([FromServices] IKycManager kycManager, int requestId, KycState kycState) =>
        await kycManager.ChangeStateKyc(requestId, kycState) switch
        {
            ChangeKycStateResult.Ok => RedirectToPage("../UserPages/UserList"),
            ChangeKycStateResult.NotFound => StatusCode(StatusCodes.Status409Conflict),
            _ => throw new ArgumentOutOfRangeException()
        };
}