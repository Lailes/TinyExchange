using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;
using TinyExchange.RazorPages.Models.UserModels.DTO;

namespace TinyExchange.RazorPages.Database.Managers.SystemUser;

public interface IUserManager
{
    Task<User?> FindUserByEmailOrDefaultAsync(string email);
    Task<User?> FindUserByIdOrDefaultAsync(int userId);
    Task<User> FindUserByIdAsync(int userId);
    Task AddUserAsync(User user);
    Task<ModifyUserResult> ModifyUserAsync(UserEditInfoModel infoModel);
    Task<ModifyUserResult> ModifyUserAsync(AdminEditInfoModelModel infoModelModel);
    Task<ModifyUserResult> ModifyUserAsync(User user, KycUserRequest kycRequest);
    Task<int> UserCountAsync();
    IQueryable<User> QueryUsersAsync(int count, int skipCount, string[] systemRoles);
}

public enum AssignRoleResult : byte { Ok, UserNotFound, AdminChangeFail }
public enum ModifyUserResult : byte { Changed, UserNotFound }