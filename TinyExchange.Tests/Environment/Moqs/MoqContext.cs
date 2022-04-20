using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TinyExchange.RazorPages.Database;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;
#pragma warning disable CS8618

namespace TinyExchange.Tests.Environment.Moqs;

public class MoqContext : DbContext, IApplicationContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserBlock> Blocks { get; set; }
    public DbSet<Debit> Debits { get; set; }
    public DbSet<Withdrawal> Withdrawals { get; set; }
    public DbSet<CardInfo> CardInfos { get; set; }
    public DbSet<KycUserRequest> KycUserRequests { get; set; }
    public Task SaveAsync() => base.SaveChangesAsync();

    public Task<IDbContextTransaction> BeginTransactionAsync() => Database.BeginTransactionAsync();
    
    public MoqContext(DbContextOptions<MoqContext> options) : base(options)
    {
        Database.EnsureCreated();
        if (!Users.Any())
            Seeder.SeedUsers(this);
    }

    public void Reset()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
        Seeder.SeedUsers(this);
    }
}