// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace TinyExchange.RazorPages.Models.AmountModels;

public class CardInfo
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string ExpireDate { get; set; } = string.Empty;
    public int Cvv { get; set; }
    public string Holder { get; set; } = string.Empty;
}