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
    public DebitType DebitType { get; set; } = DebitType.ByUser;
    public DebitState DebitState { get; set; } = DebitState.InQueue;
}

public enum DebitState : byte { Confirmed, NotConfirmed, InQueue }
public enum DebitType : byte { ByUser, ByFoundsManager }