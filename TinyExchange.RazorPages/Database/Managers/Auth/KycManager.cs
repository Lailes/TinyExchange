using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;

namespace TinyExchange.RazorPages.Database.Managers.Auth;

public class KycManager : IKycManager
{
    private readonly IUserManager _userManager;

    public KycManager(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task AddKycRequestInQueueAsync(KycUserRequest kycRequest, int userId)
    {
        var user = await _userManager.FindUserByIdAsync(userId, false);
        user.KycRequest = kycRequest;
        await _userManager.ModifyUserAsync(user);
    }
}