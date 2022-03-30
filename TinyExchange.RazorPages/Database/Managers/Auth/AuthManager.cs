using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.Auth;

public class AuthManager : IAuthManager
{
    private readonly IUserManager _userManager;
    private readonly IBlockingManager _blockingManager;
    
    public AuthManager(IUserManager userManager, IBlockingManager blockingManager)
    {
        _userManager = userManager;
        _blockingManager = blockingManager;
    }

    public async Task<LoginResult> LoginAsync(LoginData loginData, HttpContext httpContext)
    {
        var user = await _userManager.FindUserByEmailOrDefaultAsync(loginData.Email, false);
        if (user == null || user.PasswordHash != ComputeHash(loginData.Password))
            return new WrongLoginResult();

        if (await _blockingManager.GetUserBlockAsync(user.Id) != null)
            return new BannedResult();
        
        var claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()), 
            new(ClaimTypes.Role, user.Role),
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Email, user.Email),
            new(KycClaimSettings.ClaimType, user.KycStatus.ToString())
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await httpContext.SignInAsync(claimsPrincipal);

        return user.KycStatus switch
        {
            KycStatus.Confiremed => new OkLoginResult(user),
            KycStatus.Rejected => new KycIsNotConfirmedResult(user),
            KycStatus.InQueue => new KycIsInQueueResult(user),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public async Task<SignUpResult> SignUpAsync(SignUpData signUpData, HttpContext httpContext)
    {
        if (await _userManager.FindUserByEmailOrDefaultAsync(signUpData.Email) != null)
            return new EmailAlreadyRegisteredResult();

        var user = new User
        {
            FirstName = signUpData.FirstName,
            LastName = signUpData.LastName,
            Email = signUpData.Email,
            RegisteredAt = DateTime.UtcNow,
            PasswordHash = ComputeHash(signUpData.Password),
            Role = SystemRoles.User
        };
        await _userManager.AddUserAsync(user);

        await LoginAsync(new LoginData(signUpData.Email, signUpData.Password), httpContext);
        return new OkSignUpResult(user);
    }

    public async Task SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync();
    }

    public IList<string> GetAvailableSystemRoles() => SystemRoles.AllRoles;

    private static string ComputeHash(string line)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.Default.GetBytes(line));
        return Encoding.Default.GetString(hash);
    }
}