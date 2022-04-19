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
    
    [Column("ban_time")] 
    public DateTime BanTime { get; set; }
    
    [Column("release_time")] 
    public DateTime ReleaseTime { get; set; }

    public bool IsActive(DateTime now) => ReleaseTime <= now;
}