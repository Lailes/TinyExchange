#pragma warning disable CS8618
namespace TinyExchange.RazorPages.Infrastructure.Authentication;

public sealed class KycUserRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string PasportNumber { get; set; }
    public string Adress { get; set; }
    public KycState KycState { get; set; } = KycState.InQueue;
}

public enum KycState : byte { Confirmed, Rejected, InQueue }

