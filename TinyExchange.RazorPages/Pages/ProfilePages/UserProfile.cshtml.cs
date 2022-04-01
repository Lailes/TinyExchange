using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Infrastructure.Extensions;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Pages.ProfilePages;

[Authorize(Roles = SystemRoles.User, Policy = KycClaimSettings.PolicyName)]
public class UserProfile : ProfilePage
{
    public string? ErrorMessage { get; set; }
    
    public UserProfile(IUserManager userManager, IBlockingManager blockingManager, IAuthManager authManager, IAmountManager amountManager) 
        : base(userManager, blockingManager, authManager, amountManager) { }
    
    public async Task<AmountInfo> GetAmountInfo() => await AmountManager.GetAmountInfoForUser(User.GetUserId());

    public async Task OnGetSelfProfileWithMessage(string? message = null)
    {
        ErrorMessage = message;
        await OnGetSelfProfile();
    }

    // userId may be is not necessary
    public async Task<IActionResult> OnPostMakeDebit(Debit debit, CardInfo cardInfo, int userId)
    {
        debit.User = new User {Id = userId};
        debit.Card = cardInfo;
        ErrorMessage = await AmountManager.CreateDebitAsync(debit) switch
        {
            DebitResult.Ok => null,
            DebitResult.Banned => "User Is Banned",
            _ => throw new ArgumentOutOfRangeException()
        };
        ViewerUser = UserForView = await UserManager.FindUserByIdAsync(userId);
        return RedirectToPage("../ProfilePages/UserProfile",  "SelfProfileWithMessage", new { message = ErrorMessage });
    }

    public async Task<IActionResult> OnPostMakeWithdrawal(Withdrawal withdrawal, int userId)
    {
        ViewerUser = UserForView = withdrawal.User = await UserManager.FindUserByIdAsync(userId);
        ErrorMessage = await AmountManager.CreateWithdrawal(withdrawal) switch
        {
            WithdrawalResult.Ok => null,
            WithdrawalResult.Banned => "User is Banned",
            WithdrawalResult.FailNoAmount => "Insufficient Funds in the Account",
            _ => throw new ArgumentOutOfRangeException()
        };

        return RedirectToPage("../ProfilePages/UserProfile", "SelfProfileWithMessage",new { message = ErrorMessage });
    }
}