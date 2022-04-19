using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Database.Managers.Amount;

public class AmountManager : IAmountManager
{
    private readonly IApplicationContext _context;
    private readonly IUserManager _userManager;
    private readonly IBlockingManager _blockingManager;
    
    public AmountManager(IApplicationContext context, IUserManager userManager, IBlockingManager blockingManager)
    {
        _context = context;
        _userManager = userManager;
        _blockingManager = blockingManager;
    }

    public Task<Debit?> FindDebitByIdOrDefaultAsync(int transferId) => 
        _context
            .Debits
            .Include(d => d.User)
            .Include(d => d.Card)
            .FirstOrDefaultAsync(d => d.Id == transferId);

    public Task<Withdrawal?> FindWithdrawalByIdOrDefaultAsync(int transferId) => 
        _context
            .Withdrawals
            .Include(w => w.User)
            .FirstOrDefaultAsync(d => d.Id == transferId);

    public async Task<DebitResult> CreateDebitAsync(Debit debit)
    {
        await using var transaction = await _context.BeginTransactionAsync();
        
        if (await _blockingManager.CheckIsUserBlockedAsync(debit.User.Id))
            return DebitResult.Banned;

        debit.DateTime = DateTime.UtcNow;
        debit.Card = await UpdateCard(debit.Card);
        debit.User = await _userManager.FindUserByIdAsync(debit.User.Id);
        _context.Debits.Add(debit);
        
        await _context.SaveAsync();
        await transaction.CommitAsync();
        
        return DebitResult.Ok;
    }

    private async Task<CardInfo?> UpdateCard(CardInfo? cardInfo)
    {
        if (cardInfo == null)
            return null;
        
        var dbCard = await _context.CardInfos.FirstOrDefaultAsync(c => c.CardNumber == cardInfo.CardNumber);
        if (dbCard == null)
            return cardInfo;

        dbCard.Cvv = cardInfo.Cvv;
        dbCard.Holder = cardInfo.Holder;
        dbCard.ExpireDate = cardInfo.ExpireDate;
        await _context.SaveAsync();
        return dbCard;
    }

    public async Task<WithdrawalResult> CreateWithdrawal(Withdrawal withdrawal)
    {
        if (await _blockingManager.CheckIsUserBlockedAsync(withdrawal.User.Id))
            return WithdrawalResult.Banned;

        await using var transaction = await _context.BeginTransactionAsync();
        
        var userAmountInfo = await GetAmountInfoForUser(withdrawal.User.Id);
        if (userAmountInfo.Amount < withdrawal.Amount)
            return WithdrawalResult.FailNoAmount;
        
        withdrawal.DateTime = DateTime.UtcNow;
        withdrawal.User = await _userManager.FindUserByIdAsync(withdrawal.User.Id);
        _context.Withdrawals.Add(withdrawal);
        
        await _context.SaveAsync();
        await transaction.CommitAsync();
        
        return WithdrawalResult.Ok;
    }

    public async Task<AmountInfo> GetAmountInfoForUser(int userId)
    {
        var debits = await QueryDebitsForUser(userId).ToListAsync();
        var withdrawals = await QueryWithdrawalsForUser(userId).ToListAsync();
        var user = await _userManager.FindUserByIdAsync(userId);
        
        return new AmountInfo(
            amount: user.Amount,
            debitsCount: new Dictionary<DebitState, int>
            {
                {DebitState.Confirmed, debits.Count(d => d.DebitState == DebitState.Confirmed)},
                {DebitState.NotConfirmed, debits.Count(d => d.DebitState == DebitState.NotConfirmed)},
                {DebitState.InQueue, debits.Count(d => d.DebitState == DebitState.InQueue)}
            },
            withdrawalsCount: new Dictionary<WithdrawalState, int>
            {
                {WithdrawalState.Confirmed, withdrawals.Count(w => w.WithdrawalState == WithdrawalState.Confirmed)},
                {WithdrawalState.NotConfirmed, withdrawals.Count(w => w.WithdrawalState == WithdrawalState.NotConfirmed)},
                {WithdrawalState.InQueue, withdrawals.Count(w => w.WithdrawalState == WithdrawalState.InQueue)}
            }
        );
    }

    public async Task<DebitCancelResult> CancelDebitAsync(int transferId, int cancelerId)
    {
        var canceler = await _userManager.FindUserByIdAsync(cancelerId);
        var debit = await FindDebitByIdOrDefaultAsync(transferId);
        if (debit == null) 
            return DebitCancelResult.NotFound;
        
        if (!SystemRoles.IsTransferManager(canceler.Role) && debit.User.Id != canceler.Id)
            return DebitCancelResult.NotAllowed;

        debit.DebitState = DebitState.NotConfirmed;
        await _context.SaveAsync();
        return DebitCancelResult.Ok;
    }

    public async Task<WithdrawalCancelResult> CancelWithdrawalAsync(int transferId, int cancelerId)
    {
        var canceler = await _userManager.FindUserByIdAsync(cancelerId);
        var withdrawal = await FindWithdrawalByIdOrDefaultAsync(transferId);
        if (withdrawal == null)
            return WithdrawalCancelResult.NotFound;

        if (!SystemRoles.IsTransferManager(canceler.Role) && withdrawal.User.Id != canceler.Id)
            return WithdrawalCancelResult.NotAllowed;

        withdrawal.WithdrawalState = WithdrawalState.NotConfirmed;
        await _context.SaveAsync();
        return WithdrawalCancelResult.Ok;
    }

    public async Task<ConfirmDebitResult> ConfirmDebitAsync(int transferId, int confirmerId)
    {
        if (!SystemRoles.IsTransferManager((await _userManager.FindUserByIdAsync(confirmerId)).Role))
            return ConfirmDebitResult.NotAllowed;

        await using var transaction = await _context.BeginTransactionAsync();
        
        var debit = await FindDebitByIdOrDefaultAsync(transferId);
        if (debit == null) return ConfirmDebitResult.NotFound;
        
        debit.DebitState = DebitState.Confirmed;
        debit.User.Amount += debit.Amount;
        
        await _context.SaveAsync();
        await transaction.CommitAsync();
        
        return ConfirmDebitResult.Ok;
    }

    public async Task<ConfirmWithdrawalResult> ConfirmWithdrawalAsync(int transferId, int confirmerId)
    {
        if (!SystemRoles.IsTransferManager((await _userManager.FindUserByIdAsync(confirmerId)).Role))
            return ConfirmWithdrawalResult.NotAllowed;

        await using var transaction = await _context.BeginTransactionAsync();
        
        var withdrawal = await FindWithdrawalByIdOrDefaultAsync(transferId);
        if (withdrawal == null) return ConfirmWithdrawalResult.NotFound;

        withdrawal.WithdrawalState = WithdrawalState.Confirmed;
        withdrawal.User.Amount -= withdrawal.Amount;
        
        await _context.SaveAsync();
        await transaction.CommitAsync();
        
        return ConfirmWithdrawalResult.Ok;
    }

    public IQueryable<Debit> QueryDebitsForUser(int userId, DebitState[]? stateFilter = null) =>
        _context
            .Debits
            .Include(d => d.User)
            .Where(d => d.User.Id == userId && (stateFilter == null || stateFilter.Contains(d.DebitState)))
            .OrderBy(d => d.Id)
            .Include(d => d.Card);

    public IQueryable<Withdrawal> QueryWithdrawalsForUser(int userId, WithdrawalState[]? stateFilter = null) =>
        _context
            .Withdrawals
            .Include(w => w.User)
            .Where(w => w.User.Id == userId && (stateFilter == null || stateFilter.Contains(w.WithdrawalState)))
            .OrderBy(w => w.Id);
    
    public IQueryable<Debit> QueryDebits(DebitState[]? stateFilter = null) =>
        _context
            .Debits
            .Where(d => stateFilter == null || stateFilter.Contains(d.DebitState))
            .Include(d => d.User)
            .Include(d => d.Card)
            .OrderBy(d => d.Id)
            .Where(d => d.Card != null);

    public IQueryable<Withdrawal> QueryWithdrawals(WithdrawalState[]? stateFilter = null) =>
        _context
            .Withdrawals
            .Where(w => stateFilter == null || stateFilter.Contains(w.WithdrawalState))
            .Include(w => w.User)
            .OrderBy(w => w.Id);

    public async Task<AddDebitResult> AddAmount(decimal amount, int userId)
    {
        if (await _blockingManager.CheckIsUserBlockedAsync(userId)) 
            return AddDebitResult.UserIsBanned;

        _context.Debits.Add(new Debit
        {
            Amount = amount, 
            User = await _userManager.FindUserByIdAsync(userId),
            DateTime = DateTime.UtcNow,
            DebitState = DebitState.Confirmed,
            DebitType = DebitType.ByFundsManager
        });
        await _context.SaveAsync();
        return AddDebitResult.Ok;
    }
}