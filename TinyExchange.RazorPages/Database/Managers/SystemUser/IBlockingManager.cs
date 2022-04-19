using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.SystemUser;

public interface IBlockingManager
{
    Task<BlockUserResult> BlockUserAsync(int userId, DateTime releaseTime, string reason);
    Task<BlockUserResult> UnblockUserAsync(int userId);
    Task<bool> CheckIsUserBlockedAsync(int userId);
}

public enum BlockUserResult : byte
{
    UserNotFound,
    Blocked,
    Unblocked,
    AlreadyBlocked,
    UserNotBlocked,
    AdminNotFound
}