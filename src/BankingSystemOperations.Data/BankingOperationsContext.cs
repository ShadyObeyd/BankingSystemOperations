using BankingSystemOperations.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemOperations.Data;

public class BankingOperationsContext : DbContext
{
    public BankingOperationsContext(DbContextOptions options) : base(options) { }

    public DbSet<Partner> Partners { get; set; }

    public DbSet<Merchant> Merchants { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.ExternalId)
            .IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }
}