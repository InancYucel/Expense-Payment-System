using System.ComponentModel.DataAnnotations.Schema;
using Base.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entity;

[Table("Staff", Schema = "dbo")]
public class Staff : BaseEntity
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int StaffNumber { get; set; }
    public string IdentityNumber { get; set; }
    public string IBAN { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastActivityDate { get; set; }
}

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.Property(x => x.StaffNumber).IsRequired(true).ValueGeneratedNever();
        builder.Property(x => x.IdentityNumber).IsRequired(true).HasMaxLength(11);
        builder.Property(x => x.IBAN).IsRequired(true);
        builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.LastActivityDate).IsRequired(true);
        
        builder.HasIndex(x => x.IdentityNumber).IsUnique(true);
        builder.HasIndex(x => x.StaffNumber).IsUnique(true);
        builder.HasKey(x => x.StaffNumber);
    }
}