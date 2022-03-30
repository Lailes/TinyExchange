using TinyExchange.RazorPages.Infrastructure.Authentication;

namespace TinyExchange.RazorPages.Database.Managers.Auth;

public interface IKycManager
{
    public Task AddKycRequestInQueueAsync(KycUserRequest kycRequest, int userId);
}