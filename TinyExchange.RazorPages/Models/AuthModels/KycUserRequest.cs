using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace TinyExchange.RazorPages.Models.AuthModels;

[Table("kyc_user_request")]
public sealed class KycUserRequest
{
    [Column("id")] [Key] public int Id { get; set; }
    [Column("name")] public string Name { get; set; }
    [Column("last_name")] public string LastName { get; set; }
    [Column("passport_number")] public string PassportNumber { get; set; }
    [Column("address")] public string Address { get; set; }
    [Column("kyc_state")] public KycState KycState { get; set; } = KycState.InQueue;
}

public enum KycState : byte { Confirmed, Rejected, InQueue }

