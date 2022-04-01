// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinyExchange.RazorPages.Models.AmountModels;

[Table("card_info")]
public class CardInfo
{
    [Column("card_number")][Key]
    [Required]
    public string CardNumber { get; set; } = string.Empty;
    
    [Column("expire_date")] 
    [MinLength(5)]
    [MaxLength(5)]
    [RegularExpression(@"\d{2}/\d{2}")]
    [Required]
    public string ExpireDate { get; set; } = string.Empty;
     
    [Column("cvv")]
    [Range(0, 999)]
    [Required]
    public int Cvv { get; set; }
    
    [Column("holder")]
    [Required]
    public string Holder { get; set; } = string.Empty;
}