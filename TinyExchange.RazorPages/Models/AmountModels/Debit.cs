using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TinyExchange.RazorPages.Models.AmountModels.DTO;
using TinyExchange.RazorPages.Models.UserModels;
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618

namespace TinyExchange.RazorPages.Models.AmountModels;

[Table("debit")]
public class Debit
{
    [Column("id")] [Key] public int Id { get; set; }
    [ForeignKey("user_id")] public User User { get; set; }
    [Column("amount")] public decimal Amount { get; set; }
    [ForeignKey("card_id")] public CardInfo? Card { get; set; }
    [Column("date_time")] public DateTime DateTime { get; set; }
    [Column("debit_type")] public DebitType DebitType { get; set; }
    [Column("debit_state")] public DebitState DebitState { get; set; }

    public static Debit FromModel(DebitModel model, User user, CardInfo cardInfo) =>
        new()
        {
            Id = model.Id,
            Amount = model.Amount,
            DateTime = model.DateTime,
            DebitState = model.DebitState,
            DebitType = model.DebitType,
            User = user,
            Card = cardInfo
        };
}

public enum DebitState : byte { Confirmed, NotConfirmed, InQueue }
public enum DebitType : byte { ByUser, ByFundsManager }