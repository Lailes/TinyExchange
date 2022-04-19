using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618

namespace TinyExchange.RazorPages.Database;

public sealed class ApplicationContext : DbContext, IApplicationContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserBlock> Blocks { get; set; }
    public DbSet<Debit> Debits { get; set; }
    public DbSet<Withdrawal> Withdrawals { get; set; } 
    public DbSet<CardInfo> CardInfos { get; set; }
    public DbSet<KycUserRequest> KycUserRequests { get; set; }
    
    public Task SaveAsync() => base.SaveChangesAsync();
    public Task<IDbContextTransaction> BeginTransactionAsync() => Database.BeginTransactionAsync();

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
        if (Users != null && !Users.Any()) 
            Seeder.SeedUsers(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(_ => _.Blocks)
            .WithOne(_ => _.User);
    }
}