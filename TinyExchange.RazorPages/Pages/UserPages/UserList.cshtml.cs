using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Models.UserModels;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Extensions;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Pages.UserPages;

public class UserList : PageModel
{
    public User? Viewer { get; set; }
    
    public int TotalUsersCount { get; private set; }

    public int ItemsPerPage { get; private set; } = 8;

    public IEnumerable<User> Users { get; set; } = Enumerable.Empty<User>(); 
    
    public int CurrentPageNumber { get; private set; }
    
    public async Task OnGet([FromServices] IUserManager userManager, int page = 0)
    {
        Viewer = await userManager.FindUserByIdAsync(User.GetUserId());
        Users = await userManager
            .QueryUsersAsync(ItemsPerPage, ItemsPerPage * page, SystemRoles.AvailableViewRolesForRole(Viewer.Role))
            .ToListAsync();
        TotalUsersCount = await userManager.UserCountAsync();
        CurrentPageNumber = page;
    }
}