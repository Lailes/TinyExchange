using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database;

public sealed class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserBlock> Blocks { get; set; }
    

    public DbSet<Debit> Debits { get; set; }

    public DbSet<Withdrawal> Withdrawals { get; set; } 

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) => Database.EnsureCreated();
}