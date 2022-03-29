using TinyExchange.RazorPages.Models.AmountModels;

namespace TinyExchange.RazorPages.Database.Managers.Amount;

public interface IAmountManager
{
    Task<Debit?> FindDebitByIdOrDefaultAsync(int transferId);
    Task<DebitResult> CreateDebitAsync(Debit debit);
    Task<WithdrawalResult> CreateWithdrawal(Withdrawal withdrawal);
    Task<AmountInfo> GetAmountInfoForUser(int userId);
    Task<DebitCancelResult> CancelDebitTransferAsync(int transferId, int cancelerId);
    Task<WithdrawalCancelResult> CancelWithdrawalTransferAsync(int transferId, int cancelerId);
    Task<IList<Debit>> ListDebitsForUser(int userId, DebitState stateFilter);
    Task<IList<Withdrawal>> ListWithdrawalsForUser(int userId, WithdrawalState stateFilter);
    Task<AddDebitResult> AddAmount(decimal amount, int foundsManagerId, int userId);
}

public enum DebitResult : byte { Ok, Fail }
public enum WithdrawalResult : byte { Ok, FailNoAmount, Banned }

public enum DebitCancelResult : byte { Ok, NotFound, NotAllowed }
public enum WithdrawalCancelResult : byte { Ok, NotFound, NotAllowed }

public enum AddDebitResult : byte { Ok, UserIsBanned }

