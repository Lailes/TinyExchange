using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TinyExchange.RazorPages.Models.AuthModels.DTO;
using TinyExchange.RazorPages.Pages.AuthPages;

#pragma warning disable CS8618
namespace TinyExchange.RazorPages.Models.AuthModels;

[Table("kyc_user_request")]
public sealed class KycUserRequest
{
    [Column("id")] 
    [Key] 
    public int Id { get; set; }
    
    [Column("name")] 
    [Required] 
    public string Name { get; set; }
    
    [Column("last_name")] 
    [Required] 
    public string LastName { get; set; }
    
    [Column("passport_number")] 
    [Required] 
    public string PassportNumber { get; set; }
    
    [Column("address")] 
    [Required] 
    public string Address { get; set; }
    
    [Column("kyc_state")] 
    public KycState KycState { get; set; } = KycState.InQueue;

    public static KycUserRequest FromModel(KycRequestModel model) =>
        new()
        {
            Id = model.Id,
            Name = model.Name,
            LastName = model.LastName,
            Address = model.LastName,
            KycState = model.KycState,
            PassportNumber = model.PassportNumber
        };
}

public enum KycState : byte { Confirmed, Rejected, InQueue }

