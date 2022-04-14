using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Models.UserModels;

[Table("user")]
public class User
{
    [Column("id")] 
    [Key] 
    public int Id { get; set; }
    
    [Column("first_name")] 
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Column("last_name")] 
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Column("email")] 
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [Column("registered_at")] 
    public DateTime RegisteredAt { get; set; }

    [Column("role")] 
    public string Role { get; set; } = "User";
    
    [Column("password_hash")] 
    public string? PasswordHash { get; set; }
    
    [ForeignKey("kyc_request_id")] 
    public KycUserRequest? KycRequest { get; set; }

    [ForeignKey("block_id")] 
    public List<UserBlock> Blocks { get; set; } = new();

    [NotMapped] public UserBlock? ActiveBlock => Blocks.FirstOrDefault(b => b.BlockState == BlockState.Blocked);

    public User RemoveSensitiveData()
    {
        PasswordHash = null;
        return this;
    }
}