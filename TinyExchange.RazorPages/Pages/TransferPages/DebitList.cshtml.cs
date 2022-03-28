using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Pages.TransferPages;

public class TransferLIst : PageModel
{
    private readonly IUserManager _userManager;
    private readonly IAmountManager _amountManager;

    public User TransfersOwner { get; set; } = Models.UserModels.User.StubUser;
    public User ViewerUser { get; set; } = Models.UserModels.User.StubUser;
    public IList<Debit> Debits { get; private set; } = Enumerable.Empty<Debit>().ToList();
    
    public TransferLIst(IUserManager userManager, IAmountManager amountManager)
    {
        _userManager = userManager;
        _amountManager = amountManager;
    }

    public async Task OnGet(int transfersOwnerId, int viewerId)
    {
        TransfersOwner = await _userManager.FindUserByIdAsync(transfersOwnerId);
        ViewerUser = await _userManager.FindUserByIdAsync(viewerId);
        Debits = await _amountManager.ListDebitsForUser(transfersOwnerId, stateFilter: DebitState.InQueue);
    }
}