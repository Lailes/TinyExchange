namespace TinyExchange.RazorPages.Models.AmountModels;

public class AmountInfo
{
    public decimal Amount { get; set; }
    public IReadOnlyDictionary<DebitState, int> DebitsCount { get; set; }
    public IReadOnlyDictionary<WithdrawalState, int> WithdrawalsCount { get; set; }
}