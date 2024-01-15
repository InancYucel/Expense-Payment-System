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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StaffConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}