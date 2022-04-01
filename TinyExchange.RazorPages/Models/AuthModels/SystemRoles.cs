namespace TinyExchange.RazorPages.Models.AuthModels;

public static class SystemRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string KycManager = "KycManager";
    public const string FondsManager = "FondsManager";

    public static bool IsAdmin(string role) => role == Admin;
    public static bool IsTransferManager(string role) => role == FondsManager;
    public static bool IsUser(string role) => role == User;
    public static bool IsKycManager(string role) => role == KycManager;

    public static IList<string> AllRoles => new List<string> {Admin, User, KycManager, FondsManager};

    public static string[] AvailableViewRolesForRole(string role) => role switch
    {
        Admin => new []{ Admin, User, KycManager, FondsManager },
        User => Array.Empty<string>(),
        KycManager => new []{ User },
        FondsManager => new []{ User },
        _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Role is not implemented")
    };
}