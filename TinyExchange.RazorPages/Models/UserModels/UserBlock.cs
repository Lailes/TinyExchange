// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618
namespace TinyExchange.RazorPages.Models.UserModels;

[Table("user_block")]
public class UserBlock
{
    [Column("id")] 
    [Key] 
    public int Id { get; set; }
    
    [Column("reason")] 
    public string Reason { get; set; }
    
    [ForeignKey("user_id")] 
    public User User { get; set; }
    
    [ForeignKey("issuer_admin_id")] 
    public User IssuerAdmin { get; set; }
    
    [ForeignKey("releaser_admin_id")] 
    public User? ReleaserAdmin { get; set; }
    
    [Column("block_state")] 
    public BlockState BlockState { get; set; }
    
    [Column("ban_time")] 
    public DateTime BanTime { get; set; }
    
    [Column("release_time")] 
    public DateTime ReleaseTime { get; set; }
}

public enum BlockState : byte { BlockActive, UnblockedByAdmin, BlockTimeIsExpired }