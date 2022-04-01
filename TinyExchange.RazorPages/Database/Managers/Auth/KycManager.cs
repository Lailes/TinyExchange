using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.Auth;

public class KycManager : IKycManager
{
    private readonly IUserManager _userManager;
    private readonly ApplicationContext _context;

    public KycManager(IUserManager userManager, ApplicationContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task AddKycRequestInQueueAsync(KycUserRequest kycRequest, int userId)
    {
        var user = await _userManager.FindUserByIdAsync(userId, false);
        user.KycRequest = kycRequest;
        await _userManager.ModifyUserAsync(user);
    }

    public IQueryable<User> QueryUsersWithRequests(KycState[]? kycStates = null) =>
        _context
            .Users
            .Include(u => u.KycRequest)
            .Where(u => kycStates == null || (u.KycRequest != null && kycStates.Contains(u.KycRequest.KycState)));
    
    public async Task<ChangeKycStateResult> ChangeStateKyc(int kycId, KycState kycState)
    {
        var request = await _context.KycUserRequests.FirstOrDefaultAsync(k => k.Id == kycId);
        if (request == null)
            return ChangeKycStateResult.NotFound;
        
        request.KycState = kycState;
        await _context.SaveChangesAsync();
        return ChangeKycStateResult.Ok;
    }
}