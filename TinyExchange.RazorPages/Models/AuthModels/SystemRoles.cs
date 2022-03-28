namespace TinyExchange.RazorPages.Models.AuthModels;

public static class SystemRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string KycManager = "KycManager";
    public const string FoundsManager = "FoundsManager";

    public static bool IsAdmin(string role) => role == Admin;

    public static bool IsTransferManager(string role) => role == FoundsManager;

    public static IList<string> AllRoles => new List<string> {Admin, User, KycManager, FoundsManager};
}