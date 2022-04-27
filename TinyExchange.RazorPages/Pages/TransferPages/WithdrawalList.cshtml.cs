using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Pages.TransferPages;

[Authorize(Policy = KycClaimSettings.PolicyName)]
public class WithdrawalList : PageModel
{
    private readonly IUserManager _userManager;
    private readonly IAmountManager _amountManager;

    public User? TransfersOwner { get; set; }
    public User? ViewerUser { get; set; }
    public IList<Withdrawal> Withdrawals { get; private set; } = Enumerable.Empty<Withdrawal>().ToList();
    
    public WithdrawalList(IUserManager userManager, IAmountManager amountManager)
    {
        _userManager = userManager;
        _amountManager = amountManager;
    }

    public async Task OnGet(int transfersOwnerId, int viewerId)
    {
        TransfersOwner = await _userManager.FindUserByIdAsync(transfersOwnerId);
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        Withdrawals = await _amountManager.QueryWithdrawalsForUser(transfersOwnerId, new [] { WithdrawalState.InQueue }).ToListAsync();
    }

    public async Task OnGetFullQueueList(int viewerId)
    {
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        Withdrawals = await _amountManager.QueryWithdrawals(stateFilter: new[] { WithdrawalState.InQueue }).ToListAsync();
    }

    public async Task OnGetTotalWithdrawalList(int viewerId)
    {
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        if (SystemRoles.IsAdmin(ViewerUser.Role))
            Withdrawals = await _amountManager.QueryWithdrawals().ToListAsync();
        else
            Response.StatusCode = StatusCodes.Status403Forbidden;
    }
    
}