using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TinyExchange.RazorPages.Models.UserModels;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Extensions;

namespace TinyExchange.RazorPages.Pages.UserPages;

public class UserList : PageModel
{
    public User Viewer { get; set; } = Models.UserModels.User.StubUser;
    
    public int TotalUsersCount { get; private set; }

    public int ItemsPerPage { get; private set; } = 8;

    public IEnumerable<User> Users { get; set; } = Enumerable.Empty<User>(); 
    
    public int CurrentPageNumber { get; private set; }
    
    public async Task OnGet([FromServices] IUserManager userManager, [FromQuery] int page = 0)
    {
        Viewer = await userManager.FindUserByIdAsync(User.GetUserIdFromClaims());
        Users = await userManager.ListUsersAsync(ItemsPerPage, ItemsPerPage * page);
        TotalUsersCount = await userManager.UserCountAsync();
        CurrentPageNumber = page;
    }
}