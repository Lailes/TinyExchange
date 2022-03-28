namespace TinyExchange.RazorPages.Models.AmountModels;

public class AmountInfo
{
    public decimal Amount { get; set; }
    public IReadOnlyDictionary<DebitState, int> DebitsCount { get; set; }
    public IReadOnlyDictionary<WithdrawalState, int> WithdrawalsCount { get; set; }

    public AmountInfo(decimal amount, IReadOnlyDictionary<DebitState, int> debitsCount, IReadOnlyDictionary<WithdrawalState, int> withdrawalsCount)
    {
        Amount = amount;
        DebitsCount = debitsCount;
        WithdrawalsCount = withdrawalsCount;
    }
}