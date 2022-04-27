using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Infrastructure.Extensions;
using TinyExchange.RazorPages.Models.UserModels;
using TinyExchange.RazorPages.Models.UserModels.DTO;

namespace TinyExchange.RazorPages.Pages.ProfilePages;

[Authorize(Policy = KycClaimSettings.PolicyName)]
public class ProfilePage : PageModel
{
    protected readonly IUserManager UserManager;
    protected readonly IBlockingManager BlockingManager;
    protected readonly IAmountManager AmountManager;

    public User? ViewerUser { get; set; }
    public User? UserForView { get; protected set; }
    public IList<string> AvailableProfileRoles { get; }
    
    public bool SelfView => ViewerUser?.Id == UserForView?.Id;
    
    public ProfilePage(IUserManager userManager, IBlockingManager blockingManager, IAuthManager authManager, IAmountManager amountManager)
    {
        UserManager = userManager;
        BlockingManager = blockingManager;
        AmountManager = amountManager;
        AvailableProfileRoles = authManager.GetAvailableSystemRoles();
    }

    public Task<IActionResult> OnGet() => OnGetSelfProfile();

    public async Task<IActionResult> OnGetSelfProfile()
    {
        ViewerUser = UserForView = await UserManager.FindUserByIdAsync(User.GetUserId());
        return Page();
    }
    
    public async Task<IActionResult> OnGetForeignProfile(int profileId)
    { 
        ViewerUser = await UserManager.FindUserByIdAsync(User.GetUserId());
        
        var profile = await UserManager.FindUserByIdOrDefaultAsync(profileId);
        if (profile == null) return NotFound();
        
        UserForView = profile;

        return Page();
    }

    public async Task<IActionResult> OnPostEditSelfUser(UserEditInfoModel editInfoModel)
    {
        await UserManager.ModifyUserAsync(editInfoModel);
        ViewerUser = UserForView = await UserManager.FindUserByIdAsync(editInfoModel.Id);
        return Page();
    }
}