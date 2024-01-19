using System.ComponentModel.DataAnnotations.Schema;
using Base.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entity;

[Table("ExpensePaymentOrder", Schema = "dbo")]
public class ExpensePaymentOrder : BaseEntity
{
    public int Id { get; set; }
    public int ExpenseId { get; set; }
    public DateTime PaymentConfirmationDate { get; set; }
    public string AccountConfirmingOrder { get; set; } 
    public string PaymentIban { get; set; }
    public bool? IsPaymentCompleted { get; set; }
    public DateTime PaymentCompletedDate { get; set; }
    public virtual Expenses Expenses { get; set; }
    public virtual FastTransaction FastTransaction { get; set; }
    public virtual SwiftTransaction SwiftTransaction { get; set; }
}

public class ExpensePaymentOrderConfiguration : IEntityTypeConfiguration<ExpensePaymentOrder>
{
    public void Configure(EntityTypeBuilder<ExpensePaymentOrder> builder)
    {
        builder.Property(x => x.Id).IsRequired(true);
        builder.Property(x => x.ExpenseId).IsRequired(true).ValueGeneratedNever();
        builder.Property(x => x.PaymentConfirmationDate).IsRequired(true);
        builder.Property(x => x.AccountConfirmingOrder).IsRequired(true).HasMaxLength(20);
        builder.Property(x => x.PaymentIban).IsRequired(true).HasMaxLength(34);
        builder.Property(x => x.IsPaymentCompleted).IsRequired(false);
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true);

        builder.HasIndex(x => x.Id).IsUnique(true);
        builder.HasIndex(x => x.ExpenseId).IsUnique(true);

        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.FastTransaction).WithOne(x => x.ExpensePaymentOrder).HasForeignKey<FastTransaction>(x => x.ExpensePaymentOrderId);
        builder.HasOne(x => x.SwiftTransaction).WithOne(x => x.ExpensePaymentOrder).HasForeignKey<SwiftTransaction>(x => x.ExpensePaymentOrderId);
    }
}