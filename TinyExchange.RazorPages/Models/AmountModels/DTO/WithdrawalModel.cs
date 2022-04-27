namespace TinyExchange.RazorPages.Models.AmountModels.DTO;

public class WithdrawalModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public WithdrawalState WithdrawalState { get; set; } = WithdrawalState.InQueue;
}
