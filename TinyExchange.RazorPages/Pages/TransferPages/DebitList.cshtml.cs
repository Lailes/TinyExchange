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
public class TransferList : PageModel
{
    private readonly IUserManager _userManager;
    private readonly IAmountManager _amountManager;

    public User? TransfersOwner { get; set; }
    public User? ViewerUser { get; set; }
    public IList<Debit> Debits { get; private set; } = Enumerable.Empty<Debit>().ToList();
    
    public TransferList(IUserManager userManager, IAmountManager amountManager)
    {
        _userManager = userManager;
        _amountManager = amountManager;
    }

    public async Task OnGet(int transfersOwnerId, int viewerId)
    {
        TransfersOwner = await _userManager.FindUserByIdAsync(transfersOwnerId);
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        Debits = await _amountManager.QueryDebitsForUser(transfersOwnerId, new[] {DebitState.InQueue}).ToListAsync();
    }

    public async Task OnGetFullQueueList(int viewerId)
    {
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        Debits = await _amountManager.QueryDebits(stateFilter: new [] {DebitState.InQueue}).ToListAsync();
    }

    public async Task OnGetTotalDebitList(int viewerId)
    {
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        if (SystemRoles.IsAdmin(ViewerUser.Role))
            Debits = await _amountManager.QueryDebits().ToListAsync();
        else
            Response.StatusCode = StatusCodes.Status403Forbidden;
    }
}