using System.ComponentModel.DataAnnotations;

namespace TinyExchange.RazorPages.Models.AmountModels.DTO;

public class CardInfoModel
{
    [Microsoft.Build.Framework.Required]
    public string CardNumber { get; set; } = string.Empty;
    
    [MinLength(5)]
    [MaxLength(5)]
    [RegularExpression(@"\d{2}/\d{2}")]
    [Microsoft.Build.Framework.Required]
    public string ExpireDate { get; set; } = string.Empty;
     
    [Range(0, 999)]
    [Required]
    public int Cvv { get; set; }
    
    [Required]
    public string Holder { get; set; } = string.Empty;
}
