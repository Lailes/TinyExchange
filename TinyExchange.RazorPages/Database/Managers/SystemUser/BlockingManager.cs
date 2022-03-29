using Microsoft.EntityFrameworkCore;
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
        var banRecord = await _context.Blocks.FirstOrDefaultAsync(_ => _.User.Id == userId && _.BlockState == BlockState.Blocked);
        if (banRecord == null)
            return BlockUserResult.UserNotBlocked;

        if (adminId == null)
            banRecord.BlockState = BlockState.AutoUnblock;
        else
        {
            var releaser = await _userManager.FindUserByIdOrDefaultAsync((int) adminId, false);
            banRecord.ReleaserAdmin = releaser;
            banRecord.BlockState = BlockState.ManualUnblock;
        }

        await _context.SaveChangesAsync();
        return BlockUserResult.Unblocked;
    }
    
    public async Task<BlockUserResult> BlockUserAsync(int userId, int adminId, DateTime releaseTime, string reason = "")
    {
        var ban = await _context.Blocks.FirstOrDefaultAsync(ban => ban.User.Id == userId && ban.BlockState == BlockState.Blocked);
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

        var block = new UserBlock
        {
            Reason = reason,
            User = user,
            IssuerAdmin = admin,
            BanTime = DateTime.UtcNow,
            ReleaseTime = releaseTime
        };
        await _context.Blocks.AddAsync(block);
        
        await _context.SaveChangesAsync();
        return BlockUserResult.Blocked;
    }
    

    public async Task<UserBlock?> GetUserBlockAsync(int userId)
    {
        var block = await _context.Blocks.FirstOrDefaultAsync(block => block.User.Id == userId && block.BlockState == BlockState.Blocked);
        if (block == null || block.BanTime >= DateTime.UtcNow)
            return block;

        block.ReleaseTime = DateTime.UtcNow;
        block.BlockState = BlockState.AutoUnblock;
        await _context.SaveChangesAsync();
        return null;
    }
}