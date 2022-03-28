using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Database.Managers.Amount;

public class AmountManager : IAmountManager
{
    private readonly ApplicationContext _context;
    private readonly IUserManager _userManager;
    private readonly IBlockingManager _blockingManager;
    
    public AmountManager(ApplicationContext context, IUserManager userManager, IBlockingManager blockingManager)
    {
        _context = context;
        _userManager = userManager;
        _blockingManager = blockingManager;
    }

    public Task<Debit?> FindDebitByIdOrDefaultAsync(int transferId) => 
        _context.Debits.FirstOrDefaultAsync(d => d.Id == transferId);

    public Task<Withdrawal?> FindWithdrawalByIdOrDefaultAsync(int transferId) => 
        _context.Withdrawals.FirstOrDefaultAsync(d => d.Id == transferId);

    public async Task<DebitResult> CreateDebitAsync(Debit debit)
    {
        if (await _blockingManager.GetUserBlockAsync(debit.User.Id) != null)
            return DebitResult.Fail;
        
        debit.DateTime = DateTime.UtcNow;
        _context.Debits.Add(debit);
        await _context.SaveChangesAsync();
        return DebitResult.Ok;
    }

    public async Task<WithdrawalResult> CreateWithdrawal(Withdrawal withdrawal)
    {
        if (await _blockingManager.GetUserBlockAsync(withdrawal.User.Id) != null)
            return WithdrawalResult.Banned;
        
        var userAmountInfo = await GetAmountInfoForUser(withdrawal.User.Id);
        if (userAmountInfo.Amount < withdrawal.Amount)
            return WithdrawalResult.FailNoAmount;
        
        withdrawal.DateTime = DateTime.UtcNow;
        _context.Withdrawals.Add(withdrawal);
        await _context.SaveChangesAsync();
        return WithdrawalResult.Ok;
    }

    public async Task<AmountInfo> GetAmountInfoForUser(int userId)
    {
        var debits = await ListDebitsForUser(userId);
        var withdrawls = await ListWithdrawalsForUser(userId);

        return new AmountInfo(
            amount: debits.Where(d => d.DebitState == DebitState.Confiremed).Sum(d => d.Amount) - withdrawls.Where(w => w.WithdrawalState == WithdrawalState.Confirmed).Sum(w => w.Amount),
            debitsCount: new Dictionary<DebitState, int>
            {
                {DebitState.Confiremed, debits.Count(d => d.DebitState == DebitState.Confiremed)},
                {DebitState.NotConfiremed, debits.Count(d => d.DebitState == DebitState.NotConfiremed)},
                {DebitState.InQueue, debits.Count(d => d.DebitState == DebitState.InQueue)}
            },
            withdrawalsCount: new Dictionary<WithdrawalState, int>
            {
                {WithdrawalState.Confirmed, withdrawls.Count(w => w.WithdrawalState == WithdrawalState.Confirmed)},
                {WithdrawalState.NotConfirmed, withdrawls.Count(w => w.WithdrawalState == WithdrawalState.NotConfirmed)},
                {WithdrawalState.InQueue, withdrawls.Count(w => w.WithdrawalState == WithdrawalState.InQueue)}
            }
        );
    }

    public async Task<DebitCancelResult> CancelDebitTransferAsync(int transferId, int cancelerId)
    {
        var canceler = await _userManager.FindUserByIdAsync(cancelerId);
        var debit = await FindDebitByIdOrDefaultAsync(transferId);
        
        if (debit == null) 
            return DebitCancelResult.NotFound;
        
        if (!SystemRoles.IsTransferManager(canceler.Role) && debit.User.Id != canceler.Id)
            return DebitCancelResult.NotAllowed;

        debit.DebitState = DebitState.NotConfiremed;
        await _context.SaveChangesAsync();
        return DebitCancelResult.Ok;
    }

    public async Task<WithdrawalCancelResult> CancelWithdrawalTransferAsync(int transferId, int cancelerId)
    {
        var canceler = await _userManager.FindUserByIdAsync(cancelerId);
        var withdrawal = await FindWithdrawalByIdOrDefaultAsync(transferId);
        if (withdrawal == null)
            return WithdrawalCancelResult.NotFound;

        if (!SystemRoles.IsTransferManager(canceler.Role) && withdrawal.User.Id != canceler.Id)
            return WithdrawalCancelResult.NotAllowed;

        withdrawal.WithdrawalState = WithdrawalState.NotConfirmed;
        await _context.SaveChangesAsync();
        return WithdrawalCancelResult.Ok;
    }

    public async Task<IList<Debit>> ListDebitsForUser(int userId, DebitState stateFilter = DebitState.Confiremed | DebitState.InQueue | DebitState.NotConfiremed) => 
        await _context
            .Debits
            .Include(d => d.User)
            .Where(d => d.User.Id == userId && (d.DebitState & stateFilter) != 0)
            .OrderBy(d => d.Id)
            .Include(d => d.Card)
            .ToListAsync();

    public async Task<IList<Withdrawal>> ListWithdrawalsForUser(int userId, WithdrawalState stateFilter = WithdrawalState.Confirmed | WithdrawalState.InQueue | WithdrawalState.NotConfirmed) =>
        await _context
            .Withdrawals
            .Include(w => w.User)
            .Where(w => w.User.Id == userId && (w.WithdrawalState & stateFilter) != 0)
            .OrderBy(w=> w.Id)
            .ToListAsync();
}