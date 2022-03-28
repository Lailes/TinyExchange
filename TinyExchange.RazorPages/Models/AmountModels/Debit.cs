using TinyExchange.RazorPages.Models.UserModels;
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618

namespace TinyExchange.RazorPages.Models.AmountModels;

public class Debit
{
    public int Id { get; set; }
    public User User { get; set; }
    public decimal Amount { get; set; }
    public CardInfo Card { get; set; }
    public DateTime DateTime { get; set; }

    public DebitState DebitState { get; set; } = DebitState.InQueue;
}

[Flags]
public enum DebitState : byte
{
    Confiremed = 0b00000001, 
    NotConfiremed = 0b00000010, 
    InQueue = 0b00000100
} 