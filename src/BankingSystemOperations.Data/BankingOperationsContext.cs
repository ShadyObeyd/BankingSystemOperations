using BankingSystemOperations.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemOperations.Data;

public class BankingOperationsContext : DbContext
{
    public BankingOperationsContext(DbContextOptions options) : base(options) { }

    public DbSet<Partner> Partners { get; set; }

    public DbSet<Merchant> Merchants { get; set; }

    public DbSet<Transaction> Transactions { get; set; }
}