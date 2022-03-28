namespace TinyExchange.RazorPages.Models.UserModels;

public class UserBlock
{
    public int Id { get; set; }
    public string Reason { get; set; }
    public User User { get; set; }
    public User IssuerAdmin { get; set; }
    public User? ReleaserAdmin { get; set; }
    public BlockState BlockState { get; set; }
    public DateTime BanTime { get; set; }
    public DateTime ReleaseTime { get; set; }
}

public enum BlockState : byte { Blocked, ManualUnblock, AutoUnblock }