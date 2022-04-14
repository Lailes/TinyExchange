using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.SystemUser;

public class BlockingManager: IBlockingManager
{
    private readonly IUserManager _userManager;
    private readonly ApplicationContext _context;

    public BlockingManager(ApplicationContext context, IUserManager userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    public async Task<BlockUserResult> UnblockUserAsync(int userId, int? adminId = null)
    {
        var banRecord = (await _userManager.FindUserByIdAsync(userId)).ActiveBlock;
        if (banRecord == null)
            return BlockUserResult.UserNotBlocked;

        if (adminId == null)
            banRecord.BlockState = BlockState.BlockTimeIsExpired;
        else
        {
            var releaser = await _userManager.FindUserByIdOrDefaultAsync(adminId.Value, false);
            banRecord.ReleaserAdmin = releaser;
            banRecord.BlockState = BlockState.UnblockedByAdmin;
        }

        await _context.SaveChangesAsync();
        return BlockUserResult.Unblocked;
    }
    
    public async Task<BlockUserResult> BlockUserAsync(int userId, int adminId, DateTime releaseTime, string reason = "")
    {
        var ban = (await _userManager.FindUserByIdAsync(userId)).ActiveBlock;
        if (ban != null)
        {
            ban.ReleaseTime = releaseTime;
            return BlockUserResult.AlreadyBlocked;
        }

        var user = await _userManager.FindUserByIdOrDefaultAsync(userId, false);
        if (user == null)
            return BlockUserResult.UserNotFound;

        var admin = await _userManager.FindUserByIdOrDefaultAsync(adminId, false);
        if (admin == null)
            return BlockUserResult.AdminNotFound;

        user.Blocks.Add(new UserBlock
        {
            Reason = reason,
            IssuerAdmin = admin,
            BanTime = DateTime.UtcNow,
            ReleaseTime = releaseTime
        });
        
        await _context.SaveChangesAsync();
        return BlockUserResult.Blocked;
    }
    

    public async Task<bool> CheckIsUserBlockedAsync(int userId)
    {
        var block = (await _userManager.FindUserByIdAsync(userId)).ActiveBlock;

        if (block is not {BlockState: BlockState.Blocked})
            return false;

        if (block.BanTime >= DateTime.UtcNow)
            return true;

        block.BlockState = BlockState.BlockTimeIsExpired;
        await _context.SaveChangesAsync();
        return false;
    }
}