namespace TinyExchange.RazorPages.Models.AuthModels;

public static class SystemRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string KycManager = "KycManager";
    public const string FoundsManager = "FoundsManager";

    public static bool IsAdmin(string role) => role == Admin;

    public static bool IsTransferManager(string role) => role == FoundsManager;
    public static bool IsUser(string role) => role == User;
    public static bool IsKycMamager(string role) => role == KycManager;

    public static IList<string> AllRoles => new List<string> {Admin, User, KycManager, FoundsManager};

    public static string[] AvailableViewRolesForRole(string role) => role switch
    {
        Admin => new []{ Admin, User, KycManager, FoundsManager },
        User => Array.Empty<string>(),
        KycManager => new []{ User },
        FoundsManager => new []{ User },
        _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Role is not implemented")
    };
}