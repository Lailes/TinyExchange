using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Pages.ProfilePages;

[Authorize(Roles = SystemRoles.FondsManager, Policy = KycClaimSettings.PolicyName)]
public class FondsManagerProfile : ProfilePage
{
    public FondsManagerProfile(IUserManager userManager, IBlockingManager blockingManager, IAuthManager authManager, IAmountManager amountManager) : 
        base(userManager, blockingManager, authManager, amountManager) { }
    
    public async Task<IActionResult> OnPostAddFonds(int userId, int fondsManagerId, decimal amount)
    {
        await AmountManager.AddAmount(amount, userId);
        return await OnGetForeignProfile(userId);
    }
}