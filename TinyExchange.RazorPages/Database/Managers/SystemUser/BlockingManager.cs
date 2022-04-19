using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.SystemUser;

public class BlockingManager: IBlockingManager
{
    private readonly IUserManager _userManager;
    private readonly IApplicationContext _context;

    public BlockingManager(IApplicationContext context, IUserManager userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    public async Task<BlockUserResult> UnblockUserAsync(int userId)
    {
        var banRecord = (await _userManager.FindUserByIdAsync(userId)).ActiveBlock;
        if (banRecord == null)
            return BlockUserResult.UserNotBlocked;

        banRecord.ReleaseTime = DateTime.UtcNow;
        
        await _context.SaveAsync();
        return BlockUserResult.Unblocked;
    }
    
    public async Task<BlockUserResult> BlockUserAsync(int userId, DateTime releaseTime, string reason = "")
    {
        var ban = (await _userManager.FindUserByIdAsync(userId)).ActiveBlock;
        if (ban != null)
        {
            ban.ReleaseTime = releaseTime;
            await _context.SaveAsync();
            return BlockUserResult.AlreadyBlocked;
        }

        var user = await _userManager.FindUserByIdOrDefaultAsync(userId);
        if (user == null)
            return BlockUserResult.UserNotFound;

        user.Blocks.Add(new UserBlock
        {
            Reason = reason,
            BanTime = DateTime.UtcNow,
            ReleaseTime = releaseTime
        });
        
        await _context.SaveAsync();
        return BlockUserResult.Blocked;
    }
    

    public async Task<bool> CheckIsUserBlockedAsync(int userId)
    {
        var block = (await _userManager.FindUserByIdAsync(userId)).ActiveBlock;

        if (block is null)
            return false;

        return block.ReleaseTime >= DateTime.UtcNow;
    }
}