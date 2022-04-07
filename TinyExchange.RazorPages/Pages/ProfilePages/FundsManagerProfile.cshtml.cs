using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Pages.ProfilePages;

[Authorize(Roles = SystemRoles.FundsManager, Policy = KycClaimSettings.PolicyName)]
public class FundsManagerProfile : ProfilePage
{
    public FundsManagerProfile(IUserManager userManager, IBlockingManager blockingManager, IAuthManager authManager, IAmountManager amountManager) : 
        base(userManager, blockingManager, authManager, amountManager) { }
    
    public async Task<IActionResult> OnPostAddFunds(int userId, int fundsManagerId, decimal amount)
    {
        await AmountManager.AddAmount(amount, userId);
        return await OnGetForeignProfile(userId);
    }
}