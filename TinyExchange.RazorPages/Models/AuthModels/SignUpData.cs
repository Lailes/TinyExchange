using System.ComponentModel.DataAnnotations;

namespace TinyExchange.RazorPages.Models.AuthModels;

public record struct SignUpData
(
    [Required] string FirstName, 
    [Required] string LastName, 
    [Required] string Email, 
    [Required] string Password
);