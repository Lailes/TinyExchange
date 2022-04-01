using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618

namespace TinyExchange.RazorPages.Models.AuthModels;

public class SignUpData
{
    [Required] 
    public string FirstName { get; set; }
    
    [Required] 
    public string LastName { get; set; }
    
    [Required] 
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}