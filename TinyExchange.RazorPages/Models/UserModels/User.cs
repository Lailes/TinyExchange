using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Pages.AuthPages;

namespace TinyExchange.RazorPages.Models.UserModels;

[Table("user")]
public class User
{
    [Column("id")] [Key] public int Id { get; set; }
    [Column("first_name")] public string FirstName { get; set; } = string.Empty;
    [Column("last_name")] public string LastName { get; set; } = string.Empty;
    [Column("email")] public string Email { get; set; } = string.Empty;
    [Column("registered_at")] public DateTime RegisteredAt { get; set; }
    [Column("role")] public string Role { get; set; } = "User";
    [Column("password_hash")] public string? PasswordHash { get; set; }
    [ForeignKey("kyc_request_id")] public KycUserRequest? KycRequest { get; set; }
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