// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinyExchange.RazorPages.Models.AmountModels;

[Table("card_info")]
public class CardInfo
{
    [Column("card_number")][Key] public string CardNumber { get; set; } = string.Empty;
    [Column("expire_date")] public string ExpireDate { get; set; } = string.Empty;
    [Column("cvv")] public int Cvv { get; set; }
    [Column("holder")] public string Holder { get; set; } = string.Empty;
}