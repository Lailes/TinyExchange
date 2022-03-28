using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Models.AmountModels;

public class Withdrawal
{
    public int Id { get; set; }
    public User User { get; set; }
    public decimal Amount { get; set; }
    
    public string CardNumber { get; set; }
    
    public DateTime DateTime { get; set; }

    public WithdrawalState WithdrawalState { get; set; } = WithdrawalState.InQueue;
}

[Flags]
public enum WithdrawalState : byte
{
    Confirmed = 0b00000001, 
    NotConfirmed = 0b00000010, 
    InQueue = 0b00000100
}