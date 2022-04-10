using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Infrastructure.Exceptions;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;
using TinyExchange.RazorPages.Models.UserModels.DTO;
using TinyExchange.RazorPages.Pages.AuthPages;

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

    public async Task<ModifyUserResult> ModifyUserAsync(UserEditInfoModel infoModel)
    {
        var databaseEntity = await FindUserByIdOrDefaultAsync(infoModel.Id, false);
        if (databaseEntity == null) 
            return ModifyUserResult.UserNotFound;
        
        databaseEntity.Email = infoModel.Email;
        databaseEntity.FirstName = infoModel.FirstName;
        databaseEntity.LastName = infoModel.LastName;
        
        await _context.SaveChangesAsync();
        return ModifyUserResult.Changed;
    }

    public async Task<ModifyUserResult> ModifyUserAsync(AdminEditInfoModelModel infoModelModel)
    {
        var databaseEntity = await FindUserByIdOrDefaultAsync(infoModelModel.Id, false);
        if (databaseEntity == null) 
            return ModifyUserResult.UserNotFound;
        
        databaseEntity.Email = infoModelModel.Email;
        databaseEntity.FirstName = infoModelModel.FirstName;
        databaseEntity.LastName = infoModelModel.LastName;
        databaseEntity.Role = infoModelModel.Role;
        
        await _context.SaveChangesAsync();
        return ModifyUserResult.Changed;
    }

    public async Task<ModifyUserResult> ModifyUserAsync(User user, KycUserRequest kycRequest)
    {
        var databaseEntity = await FindUserByIdOrDefaultAsync(user.Id, false);
        if (databaseEntity == null) 
            return ModifyUserResult.UserNotFound;

        user.KycRequest = kycRequest;
        
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