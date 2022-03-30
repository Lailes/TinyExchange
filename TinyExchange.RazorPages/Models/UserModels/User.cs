namespace TinyExchange.RazorPages.Models.UserModels;

public class User
{
    public int Id { get; set; } = 0;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public string Role { get; set; } = "User";
    public string? PasswordHash { get; set; }
    
    public KycStatus KycStatus { get; set; }
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

public enum KycStatus { InQueue, Confiremed, Rejected }