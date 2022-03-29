// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;

namespace TinyExchange.RazorPages.Models.AmountModels;

public class CardInfo
{
    [Key]
    public string CardNumber { get; set; } = string.Empty;
    public string ExpireDate { get; set; } = string.Empty;
    public int Cvv { get; set; }
    public string Holder { get; set; } = string.Empty;
}