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
        };

        if (user.KycRequest != null) 
            claims.Add(new Claim(KycClaimSettings.ClaimType, user.KycRequest.KycState.ToString()));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await httpContext.SignInAsync(claimsPrincipal);

        if (user.KycRequest == null)
            return new KycNotCreatedResult();
        
        return user.KycRequest.KycState switch
        {
            KycState.Confirmed => new OkLoginResult(user),
            KycState.Rejected => new KycIsRejectedResult(),
            KycState.InQueue => new KycIsInQueueResult(),
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

        await LoginAsync(new LoginData { Email = signUpData.Email, Password = signUpData.Password }, httpContext);
        return new OkSignUpResult(user);
    }

    public async Task SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync();
    }

    public IList<string> GetAvailableSystemRoles() => SystemRoles.AllRoles;

    public static string ComputeHash(string line)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.Default.GetBytes(line));
        return Encoding.Default.GetString(hash);
    }
}