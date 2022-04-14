using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Models.AmountModels.DTO;
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

    public static Withdrawal FromModel(WithdrawalModel model, User user) =>
        new()
        {
            Id = model.Id,
            Amount = model.Amount,
            CardNumber = model.CardNumber,
            DateTime = model.DateTime,
            WithdrawalState = model.WithdrawalState,
            User = user
        };
}

public enum WithdrawalState : byte { Confirmed, NotConfirmed, InQueue }