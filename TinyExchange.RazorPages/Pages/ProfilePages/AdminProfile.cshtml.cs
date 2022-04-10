using Microsoft.AspNetCore.Authorization;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;
using TinyExchange.RazorPages.Models.UserModels.DTO;

namespace TinyExchange.RazorPages.Pages.ProfilePages;

[Authorize(Roles = SystemRoles.Admin, Policy = KycClaimSettings.PolicyName)] 
public class AdminProfile : ProfilePage
{
    public AdminProfile(IUserManager userManager, IBlockingManager blockingManager, IAuthManager authManager, IAmountManager amountManager) : 
        base(userManager, blockingManager, authManager, amountManager) { }
    
    public async Task OnPostBlockUser(string reason, DateTime releaseTime, int userId, int adminId)
    {
        await BlockingManager.BlockUserAsync(userId, adminId, releaseTime, reason);
        ViewerUser = await UserManager.FindUserByIdAsync(adminId);
        UserForView = await UserManager.FindUserByIdAsync(userId);
    }

    public async Task OnPostUnBlockUser(int userId, int adminId)
    {
        await BlockingManager.UnblockUserAsync(userId, adminId);
        ViewerUser = await UserManager.FindUserByIdAsync(adminId);
        UserForView = await UserManager.FindUserByIdAsync(userId);
    }
    
    public async Task OnPostEditForeignUser(AdminEditInfoModelModel user, int adminId)
    {
        await UserManager.ModifyUserAsync(user);
        ViewerUser = await UserManager.FindUserByIdAsync(adminId); 
        UserForView = await UserManager.FindUserByIdAsync(user.Id);
    }
}