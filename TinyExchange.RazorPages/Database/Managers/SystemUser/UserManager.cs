using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Infrastructure.Exceptions;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.SystemUser;

public class UserManager : IUserManager
{
    private readonly ApplicationContext _context;

    public UserManager(ApplicationContext context) =>
        _context = context;

    public async Task<User?> FindUserByEmailOrDefaultAsync(string email, bool anonimize = true) =>
        anonimize
            ? (await _context.Users.Include(u => u.KycRequest).FirstOrDefaultAsync(user => user.Email == email))?.RemoveSensitiveData()
            : await _context.Users.Include(u => u.KycRequest).FirstOrDefaultAsync(user => user.Email == email);

    public async Task<User?> FindUserByIdOrDefaultAsync(int id, bool anonimize = true) =>
        anonimize
            ? (await _context.Users.Include(u => u.KycRequest).FirstOrDefaultAsync(user => user.Id == id))?.RemoveSensitiveData()
            : await _context.Users.Include(u => u.KycRequest).FirstOrDefaultAsync(user => user.Id == id);

    public async Task<User> FindUserByEmailAsync(string email, bool anonimize = true) =>
        await FindUserByEmailOrDefaultAsync(email, anonimize) ??
        throw new UserNotFoundException($"User with email = \"{email}\" not found");

    public async Task<User> FindUserByIdAsync(int userId, bool anonimize = true) => 
        await FindUserByIdOrDefaultAsync(userId, anonimize) ?? 
        throw new UserNotFoundException($"User with ID = {userId} not found");

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<ModifyUserResult> ModifyUserAsync(User user)
    {
        var databaseEntity = await FindUserByIdOrDefaultAsync(user.Id, false);
        if (databaseEntity == null) 
            return ModifyUserResult.UserNotFound;
        
        databaseEntity.Email = user.Email;
        databaseEntity.FirstName = user.FirstName;
        databaseEntity.LastName = user.LastName;
        databaseEntity.Role = user.Role;
        databaseEntity.KycRequest ??= user.KycRequest;
        await _context.SaveChangesAsync();
        return ModifyUserResult.Changed;
    }

    public async Task<AssignRoleResult> AssignRole(int userId, string role)
    {
        var user = await FindUserByIdOrDefaultAsync(userId);
        if (user == null) 
            return AssignRoleResult.UserNotFound;

        if (SystemRoles.IsAdmin(user.Role))
            return AssignRoleResult.AdminChangeFail;
        
        user.Role = role;
        await _context.SaveChangesAsync();
        return AssignRoleResult.Ok;
    }

    public IQueryable<User> QueryUsersAsync(int count, int skipCount, string[] systemRoles) =>
        _context
            .Users
            .OrderBy(user => user.Id)
            .Where(u => systemRoles.Contains(u.Role))
            .Skip(skipCount)
            .Take(count)
            .Select(user => user.RemoveSensitiveData());

    public Task<int> UserCountAsync() => _context.Users.CountAsync();
    
}