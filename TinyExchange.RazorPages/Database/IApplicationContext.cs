using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;
using TinyExchange.RazorPages.Models.UserModels;

namespace TinyExchange.RazorPages.Database;

public interface IApplicationContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserBlock> Blocks { get; set; }
    DbSet<Debit> Debits { get; set; }
    DbSet<Withdrawal> Withdrawals { get; set; }
    DbSet<CardInfo> CardInfos { get; set; }
    DbSet<KycUserRequest> KycUserRequests { get; set; }

    public Task SaveAsync();

    public Task<IDbContextTransaction> BeginTransactionAsync();
}