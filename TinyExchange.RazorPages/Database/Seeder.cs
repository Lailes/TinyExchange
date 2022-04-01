using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database;

public static class Seeder
{
    public static void SeedUsers(ApplicationContext? context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        
        var userAdmin = new User
        {
            FirstName = "Adam", LastName = "Jensen",
            Email = "admin", PasswordHash = AuthManager.ComputeHash("1"),
            RegisteredAt = DateTime.UtcNow,
            Role = SystemRoles.Admin,
            KycRequest = new KycUserRequest
            {
                Address = "Some Where",
                KycState = KycState.Confirmed,
                Name = "Adam",
                LastName = "Jensen",
                PassportNumber = "123456789"
            }
        };
        context.Add(userAdmin);
        context.SaveChanges();

        var userUser = new User
        {
            FirstName = "Bob", LastName = "Ross",
            Email = "user", PasswordHash = AuthManager.ComputeHash("1"),
            RegisteredAt = DateTime.UtcNow,
            Role = SystemRoles.User,
            KycRequest = new KycUserRequest
            {
                Address = "Some Where",
                KycState = KycState.Confirmed,
                Name = "Bob",
                LastName = "Ross",
                PassportNumber = "1234"
            }
        };
        context.Add(userUser);
        context.SaveChanges();

        var userFonds = new User
        {
            FirstName = "Morgan", LastName = "Yu",
            Email = "fonds", PasswordHash = AuthManager.ComputeHash("1"),
            RegisteredAt = DateTime.UtcNow,
            Role = SystemRoles.FondsManager,
            KycRequest = new KycUserRequest
            {
                Address = "Some Where",
                KycState = KycState.Confirmed,
                Name = "Morgan",
                LastName = "Yu",
                PassportNumber = "123456"
            }
        };
        context.Add(userFonds);
        context.SaveChanges();

        var userKyc = new User
        {
            FirstName = "Sam", LastName = "Strand",
            Email = "kyc", PasswordHash = AuthManager.ComputeHash("1"),
            RegisteredAt = DateTime.UtcNow,
            Role = SystemRoles.KycManager,
            KycRequest = new KycUserRequest
            {
                Address = "Some Where",
                KycState = KycState.Confirmed,
                Name = "Sam",
                LastName = "Strand",
                PassportNumber = "123453126"
            }
        };
        context.Add(userKyc);
        context.SaveChanges();
        
    }
}