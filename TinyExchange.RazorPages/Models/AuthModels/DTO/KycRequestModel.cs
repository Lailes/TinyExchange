using Microsoft.Build.Framework;

namespace TinyExchange.RazorPages.Models.AuthModels.DTO;

public sealed class KycRequestModel
{
    public int Id { get; set; }
    
    [Required] 
    public string Name { get; set; }
    
    [Required] 
    public string LastName { get; set; }
    
    [Required] 
    public string PassportNumber { get; set; }
    
    [Required] 
    public string Address { get; set; }
    
    public KycState KycState { get; set; } = KycState.InQueue;
}
