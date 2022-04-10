using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Infrastructure.Extensions;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AmountModels.DTO;
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

    public async Task<IActionResult> OnGetSelfProfileWithMessage(string? message = null)
    {
        ErrorMessage = message;
        return await OnGetSelfProfile();
    }

    public async Task<IActionResult> OnPostMakeDebit(DebitModel debitModel, CardInfoModel cardInfoModel, int userId)
    {
        var validResultDebit = !TryValidateModel(debitModel);
        var validResultCardInfo = !TryValidateModel(cardInfoModel);
        if (validResultDebit || validResultCardInfo)
            return await OnGetSelfProfileWithMessage("Form is filled with incorrect data");

        var debit = Debit.FromModel(
            debitModel,
            new User { Id = userId },
            CardInfo.FromModel(cardInfoModel)
        );
        
        ErrorMessage = await AmountManager.CreateDebitAsync(debit) switch
        {
            DebitResult.Ok => null,
            DebitResult.Banned => "User Is Banned",
            _ => throw new ArgumentOutOfRangeException()
        };
        
        ViewerUser = UserForView = await UserManager.FindUserByIdAsync(userId);
        return RedirectToPage("../ProfilePages/UserProfile",  "SelfProfileWithMessage", new { message = ErrorMessage });
    }

    public async Task<IActionResult> OnPostMakeWithdrawal(WithdrawalModel withdrawalModel)
    {
        if (!TryValidateModel(withdrawalModel))
            return Page();
        
        var withdrawal = Withdrawal.FromModel(
            withdrawalModel,
            await UserManager.FindUserByIdAsync(withdrawalModel.UserId)
        );

        ViewerUser = UserForView = withdrawal.User;
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