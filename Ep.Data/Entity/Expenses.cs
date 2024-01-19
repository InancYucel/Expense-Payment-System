using System.ComponentModel.DataAnnotations.Schema;
using Base.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entity;

[Table("Expenses", Schema = "dbo")]
public class Expenses : BaseEntity
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public string InvoiceReferenceNumber { get; set; }
    public double InvoiceAmount { get; set; }
    public string InvoiceCurrencyType { get; set; }
    public string InvoiceCategory { get; set; }
    public string PaymentInstrument { get; set; }
    public string PaymentLocation { get; set; }
    public string ExpenseClaimDescription { get; set; }
    public string ExpenseRequestStatus { get; set; }
    public string ExpensePaymentRefusal { get; set; }
    public virtual Staff Staff { get; set; }
    public virtual ExpensePaymentOrder ExpensePaymentOrder { get; set; }
}

public class ExpensesConfiguration : IEntityTypeConfiguration<Expenses>
{
    public void Configure(EntityTypeBuilder<Expenses> builder)
    {
        builder.Property(x => x.Id).IsRequired(true);
        builder.Property(x => x.StaffId).IsRequired(true).ValueGeneratedNever();
        builder.Property(x => x.InvoiceReferenceNumber).IsRequired(true);
        builder.Property(x => x.InvoiceAmount).IsRequired(true).HasMaxLength(11);
        builder.Property(x => x.InvoiceCategory).IsRequired(true).HasMaxLength(15);
        builder.Property(x => x.PaymentInstrument).IsRequired(true).HasMaxLength(15);
        builder.Property(x => x.PaymentLocation).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.ExpenseClaimDescription).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.ExpenseRequestStatus).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.ExpensePaymentRefusal).IsRequired(false).HasMaxLength(50);
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true);

        builder.HasIndex(x => x.Id).IsUnique(true);
        builder.HasIndex(x => x.InvoiceReferenceNumber).IsUnique(true);

        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.ExpensePaymentOrder).WithOne(x => x.Expenses).HasForeignKey<ExpensePaymentOrder>(x => x.ExpenseId);
    }
}