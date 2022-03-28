using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.SystemUser;

public interface IUserManager
{
    Task<User?> FindUserByEmailOrDefaultAsync(string email, bool anonimize = true);
    Task<User?> FindUserByIdOrDefaultAsync(int userId, bool anonimize = true);
    Task<User> FindUserByEmailAsync(string email, bool anonimize = true);
    Task<User> FindUserByIdAsync(int userId, bool anonimize = true);

    Task AddUserAsync(User user);
    Task<AssignRoleResult> AssignRole(int userId, string role);
    Task<ModifyUserResult> ModifyUserAsync(User user);
    Task<IEnumerable<User>> ListUsersAsync(int count, int skipCount);
    Task<int> UserCountAsync();
}

public enum AssignRoleResult : byte { Ok, UserNotFound, AdminChangeFail }
public enum ModifyUserResult : byte { Changed, UserNotFound }