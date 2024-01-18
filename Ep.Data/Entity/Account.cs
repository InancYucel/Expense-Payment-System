using System.ComponentModel.DataAnnotations.Schema;
using Base.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entity;

[Table("Account", Schema = "dbo")]
public class Account : BaseEntity
{
    public int Id { get; set; }
    public int AccountNumber { get; set; }
    public int StaffId { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyType { get; set; }
    public string Name { get; set; }
    public DateTime OpenDate { get; set; }
    public virtual Staff Staff { get; set; }
}

//The codes we wrote to specify the fields and their properties in the database
//Migrations are created with these codes and they are added to the database.
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        // Not assigning values automatically
        builder.Property(x => x.Id).IsRequired(true);
        builder.Property(z => z.AccountNumber).ValueGeneratedNever();

        builder.Property(z => z.InsertDate).IsRequired(true);
        builder.Property(z => z.InsertUserId).IsRequired(true);

        //Fields where data entry is not required
        builder.Property(z => z.UpdateDate).IsRequired(false);
        builder.Property(z => z.UpdateUserId).IsRequired(false);

        //It will start with a default value
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true);

        builder.Property(z => z.StaffId).IsRequired(true);
        builder.Property(z => z.AccountNumber).IsRequired(true);
        builder.Property(z => z.IBAN).IsRequired(true).HasMaxLength(34);
        builder.Property(z => z.Balance).IsRequired(true).HasPrecision(18, 4);
        builder.Property(z => z.CurrencyType).IsRequired(true).HasMaxLength(100);
        builder.Property(z => z.Name).IsRequired(false).HasMaxLength(100);
        builder.Property(z => z.OpenDate).IsRequired(true);

        builder.HasIndex(x => x.Id).IsUnique(true);
        builder.HasIndex(z => z.AccountNumber).IsUnique(true);
        builder.HasKey(z => z.Id);
    }
}