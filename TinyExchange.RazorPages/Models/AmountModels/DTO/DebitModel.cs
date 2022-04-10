namespace TinyExchange.RazorPages.Models.AmountModels.DTO;

public class DebitModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public int CardId { get; set; }
    public DateTime DateTime { get; set; }
    public DebitType DebitType { get; set; } = DebitType.ByUser;
    public DebitState DebitState { get; set; } = DebitState.InQueue;
}