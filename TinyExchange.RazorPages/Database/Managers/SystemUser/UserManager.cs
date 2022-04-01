using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Infrastructure.Exceptions;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.SystemUser;

public class UserManager : IUserManager
{
    private readonly ApplicationContext _context;

    public UserManager(ApplicationContext context) =>
        _context = context;

    public async Task<User?> FindUserByEmailOrDefaultAsync(string email, bool anonimize = true)
    {
        var user = await _context
            .Users
            .Include(u => u.KycRequest)
            .FirstOrDefaultAsync(user => user.Email == email);

        return anonimize ? user?.RemoveSensitiveData() : user;
    }
    
    public async Task<User?> FindUserByIdOrDefaultAsync(int id, bool anonimize = true)
    {
        var user = await _context
            .Users
            .Include(u => u.KycRequest)
            .FirstOrDefaultAsync(user => user.Id == id);

        return anonimize ? user?.RemoveSensitiveData() : user;
    }
    
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