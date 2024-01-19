using System.ComponentModel.DataAnnotations.Schema;
using Base.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entity;
[Table("SwiftTransaction", Schema = "dbo")]

public class SwiftTransaction : BaseEntityWithId
{
    public int AccountId { get; set; }
    public int ExpensePaymentOrderId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    
    [Precision(18, 4)]
    public double Amount { get; set; }
    public string CurrencyType { get; set; }
    public string Description { get; set; }
    public string SenderBank { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
    public string ReceiverBank { get; set; }
    public string ReceiverIban { get; set; }
    public string ReceiverName { get; set; }
    
    public virtual ExpensePaymentOrder ExpensePaymentOrder { get; set; }
}

public class SwiftTransactionConfiguration : IEntityTypeConfiguration<SwiftTransaction>
{
    public void Configure(EntityTypeBuilder<SwiftTransaction> builder)
    {
        builder.Property(z => z.InsertDate).IsRequired(true);
        builder.Property(z => z.InsertUserId).IsRequired(true);
        
        //Fields where data entry is not required
        builder.Property(z => z.UpdateDate).IsRequired(false);
        builder.Property(z => z.UpdateUserId).IsRequired(false);
        
        //It will start with a default value
        builder.Property(z => z.IsActive).IsRequired(true).HasDefaultValue(true);
        
        builder.Property(x => x.Id).IsRequired(true);
        builder.Property(z => z.AccountId).IsRequired(true);
        builder.Property(z => z.TransactionDate).IsRequired(true);
        builder.Property(z => z.Amount).IsRequired(true).HasColumnType("decimal(18,4)");
        builder.Property(z => z.CurrencyType).IsRequired(true).HasMaxLength(3);
        builder.Property(z => z.Description).IsRequired(false).HasMaxLength(300);
        builder.Property(z => z.ReferenceNumber).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.SenderIban).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.SenderBank).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.SenderName).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.ReceiverIban).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.ReceiverBank).IsRequired(true).HasMaxLength(50);
        builder.Property(z => z.ReceiverName).IsRequired(true).HasMaxLength(50);

        //Indexing so that results come quickly
        builder.HasIndex(z => z.ReferenceNumber);
        builder.HasIndex(x => x.Id).IsUnique(true);
        builder.HasKey(x => x.Id);
    }
}