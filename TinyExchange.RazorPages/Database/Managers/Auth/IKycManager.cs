using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.Auth;

public interface IKycManager
{
    Task AddKycRequestInQueueAsync(KycUserRequest kycRequest, int userId);
    Task<IList<User>> ListUsersWithRequests(KycState[]? kycStates = null);
    Task<ChangeKycStateResult> ChangeStateKyc(int kycId, KycState kycState);
}
public enum ChangeKycStateResult : byte { Ok, NotFound }