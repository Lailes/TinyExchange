using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database.Managers.Auth;

public interface IAuthManager
{
    Task<LoginResult> LoginAsync(LoginData loginData, HttpContext httpContext);
    Task<SignUpResult> SignUpAsync(SignUpData signUpData, HttpContext httpContext);
    Task SignOutAsync(HttpContext httpContext);

    IList<string> GetAvailableSystemRoles();
}


public enum LoginResultType : byte { Ok, WrongLogin, Banned }
public abstract record LoginResult(LoginResultType Type);
public record OkLoginResult(User User) : LoginResult(LoginResultType.Ok);
public record WrongLoginResult() : LoginResult(LoginResultType.WrongLogin);
public record BannedResult() : LoginResult(LoginResultType.Banned);

public enum SignUpResultType : byte { Ok, EmailRegistered }


public abstract record SignUpResult(SignUpResultType Type);
public record OkSignUpResult(User User) : SignUpResult(SignUpResultType.Ok);
public record EmailAlreadyRegisteredResult() : SignUpResult(SignUpResultType.EmailRegistered);
