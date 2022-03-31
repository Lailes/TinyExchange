using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Pages.AuthPages;

namespace TinyExchange.RazorPages.Models.UserModels;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public string Role { get; set; } = "User";
    public string? PasswordHash { get; set; }
    
    public KycUserRequest? KycRequest { get; set; }
    public User RemoveSensitiveData()
    {
        PasswordHash = null;
        return this;
    }

    public static User StubUser => new() {
        RegisteredAt = DateTime.MinValue,
        Role = "Stub",
        PasswordHash = string.Empty
    };
}