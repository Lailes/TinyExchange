using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Pages.TransferPages;

public class TransferList : PageModel
{
    private readonly IUserManager _userManager;
    private readonly IAmountManager _amountManager;

    public User TransfersOwner { get; set; } = Models.UserModels.User.StubUser;
    public User ViewerUser { get; set; } = Models.UserModels.User.StubUser;
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
        Debits = await _amountManager.ListDebitsForUser(transfersOwnerId, new [] { DebitState.InQueue });
    }

    public async Task OnGetFullQueueList(int viewerId)
    {
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        Debits = await _amountManager.ListDebits(debitStates: new [] {DebitState.InQueue});
    }

    public async Task OnGetTotalDebitList(int viewerId)
    {
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        if (SystemRoles.IsAdmin(ViewerUser.Role))
            Debits = await _amountManager.ListDebits();
        else
            Response.StatusCode = StatusCodes.Status403Forbidden;
    }
}