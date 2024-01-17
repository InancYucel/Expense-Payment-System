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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StaffConfiguration());
        modelBuilder.ApplyConfiguration(new ExpensesConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}