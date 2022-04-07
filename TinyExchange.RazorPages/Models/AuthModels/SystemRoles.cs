namespace TinyExchange.RazorPages.Models.AuthModels;

public static class SystemRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string KycManager = "KycManager";
    public const string FundsManager = "FundsManager";

    public static bool IsAdmin(string role) => role == Admin;
    public static bool IsTransferManager(string role) => role == FundsManager;
    public static bool IsUser(string role) => role == User;
    public static bool IsKycManager(string role) => role == KycManager;

    public static IList<string> AllRoles => new List<string> {Admin, User, KycManager, FundsManager};

    public static string[] AvailableViewRolesForRole(string role) => role switch
    {
        Admin => new []{ Admin, User, KycManager, FundsManager },
        User => Array.Empty<string>(),
        KycManager => new []{ User },
        FundsManager => new []{ User },
        _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Role is not implemented")
    };
}