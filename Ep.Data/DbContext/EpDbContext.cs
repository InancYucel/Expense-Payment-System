using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.DbContext;

public class EpDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public EpDbContext(DbContextOptions<EpDbContext> options) : base(options)
    {
    }
    
    //DB Sets
    public DbSet<Staff> Staff { get; set; }
    public DbSet<Expenses> Expenses { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<ExpensePaymentOrder> ExpensePaymentOrders { get; set; }
    public DbSet<FastTransaction> FastTransactions { get; set; }
    public DbSet<SwiftTransaction> SwiftTransactions { get; set; }
    public DbSet<PaymentCategories> PaymentCategories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StaffConfiguration());
        modelBuilder.ApplyConfiguration(new ExpensesConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new ExpensePaymentOrderConfiguration());
        modelBuilder.ApplyConfiguration(new FastTransactionConfiguration());
        modelBuilder.ApplyConfiguration(new SwiftTransactionConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentCategoriesConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}