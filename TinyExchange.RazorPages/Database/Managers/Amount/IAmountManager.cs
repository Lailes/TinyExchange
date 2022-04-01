using TinyExchange.RazorPages.Models.AmountModels;

namespace TinyExchange.RazorPages.Database.Managers.Amount;

public interface IAmountManager
{
    Task<DebitResult> CreateDebitAsync(Debit debit);
    Task<WithdrawalResult> CreateWithdrawal(Withdrawal withdrawal);
    Task<AmountInfo> GetAmountInfoForUser(int userId);
    Task<DebitCancelResult> CancelDebitAsync(int transferId, int cancelerId);
    Task<WithdrawalCancelResult> CancelWithdrawalAsync(int transferId, int cancelerId);
    Task<ConfirmDebitResult> ConfirmDebitAsync(int transferId, int confirmerId);
    Task<ConfirmWithdrawalResult> ConfirmWithdrawalAsync(int transferId, int confirmerId);
    Task<AddDebitResult> AddAmount(decimal amount, int userId);
    IQueryable<Debit> QueryDebitsForUser(int userId, DebitState[]? stateFilter = null);
    IQueryable<Withdrawal> QueryWithdrawalsForUser(int userId, WithdrawalState[]? stateFilter = null);
    IQueryable<Debit> QueryDebits(DebitState[]? stateFilter = null);
    IQueryable<Withdrawal> QueryWithdrawals(WithdrawalState[]? stateFilter = null); 
}

public enum DebitResult : byte { Ok, Fail }
public enum WithdrawalResult : byte { Ok, FailNoAmount, Banned }
public enum DebitCancelResult : byte { Ok, NotFound, NotAllowed }
public enum WithdrawalCancelResult : byte { Ok, NotFound, NotAllowed }
public enum AddDebitResult : byte { Ok, UserIsBanned }
public enum ConfirmDebitResult : byte { Ok, NotFound, NotAllowed }
public enum ConfirmWithdrawalResult : byte { Ok, NotFound, NotAllowed }

