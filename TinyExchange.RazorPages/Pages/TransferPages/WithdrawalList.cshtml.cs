using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Pages.TransferPages;

public class WithdrawalList : PageModel
{
    private readonly IUserManager _userManager;
    private readonly IAmountManager _amountManager;

    public User TransfersOwner { get; set; } = Models.UserModels.User.StubUser;
    public User ViewerUser { get; set; } = Models.UserModels.User.StubUser;
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
        Withdrawals = await _amountManager.ListWithdrawalsForUser(transfersOwnerId, new [] { WithdrawalState.InQueue });
    }

    public async Task OnGetFullQueueList(int viewerId)
    {
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        Withdrawals = await _amountManager.ListWithdrawals(withdrawalStates: new[] { WithdrawalState.InQueue });
    }

    public async Task OnGetTotalWithdrawalList(int viewerId)
    {
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        if (SystemRoles.IsAdmin(ViewerUser.Role))
            Withdrawals = await _amountManager.ListWithdrawals();
        else
            Response.StatusCode = StatusCodes.Status403Forbidden;
    }
    
}