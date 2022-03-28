namespace TinyExchange.RazorPages.Models.AmountModels;

public class CardInfo
{
    public int Id { get; set; }
    public string CardNumber { get; set; }
    public string ExpireDate { get; set; }
    public int CVV { get; set; }
    public string Holder { get; set; }
}