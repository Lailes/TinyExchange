using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TinyExchange.RazorPages.Models.UserModels;
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618

namespace TinyExchange.RazorPages.Models.AmountModels;

[Table("withdrawal")]
public class Withdrawal
{
    [Column("id")] [Key] public int Id { get; set; }
    [ForeignKey("user_id")] public User User { get; set; }
    [Column("amount")] public decimal Amount { get; set; }
    [Column("card_number")] public string CardNumber { get; set; } = string.Empty;
    [Column("date_time")] public DateTime DateTime { get; set; }
    [Column("withdrawal_state")] public WithdrawalState WithdrawalState { get; set; } = WithdrawalState.InQueue;
}

[Flags]
public enum WithdrawalState : byte
{
    Confirmed = 0b00000001, 
    NotConfirmed = 0b00000010, 
    InQueue = 0b00000100
}