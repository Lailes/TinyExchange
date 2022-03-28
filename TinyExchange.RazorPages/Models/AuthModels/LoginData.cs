using System.ComponentModel.DataAnnotations;

namespace TinyExchange.RazorPages.Models.AuthModels;

public record LoginData 
(
    [Required] string Email, 
    [Required] string Password
);