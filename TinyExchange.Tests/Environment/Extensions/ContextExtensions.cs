using System.Threading.Tasks;
using TinyExchange.RazorPages.Database;

namespace TinyExchange.Tests.Environment.Extensions;

public static class ContextExtensions
{
    public static async Task SeedAsync(this ApplicationContext applicationContext) => 
        await Seeder.SeedUsersAsync(applicationContext);

    public static async Task ClearAsync(this ApplicationContext applicationContext)
    {
        applicationContext.Debits.RemoveRange(applicationContext.Debits);
        applicationContext.Blocks.RemoveRange(applicationContext.Blocks);
        applicationContext.Withdrawals.RemoveRange(applicationContext.Withdrawals);
        applicationContext.CardInfos.RemoveRange(applicationContext.CardInfos);
        applicationContext.KycUserRequests.RemoveRange(applicationContext.KycUserRequests);
        applicationContext.Users.RemoveRange(applicationContext.Users);

        await applicationContext.SaveChangesAsync();
    }
}