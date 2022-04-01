using System.ComponentModel.DataAnnotations;

namespace TinyExchange.RazorPages.Models.AuthModels;

public class LoginData
{
    [Required] 
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}