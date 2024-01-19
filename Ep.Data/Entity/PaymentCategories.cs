using System.ComponentModel.DataAnnotations.Schema;
using Base.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entity;

[Table("PaymentCategories", Schema = "dbo")]
public class PaymentCategories : BaseEntityWithId
{
    public string Category { get; set; }
}

public class PaymentCategoriesConfiguration : IEntityTypeConfiguration<PaymentCategories>
{
    public void Configure(EntityTypeBuilder<PaymentCategories> builder)
    {
        // Not assigning values automatically
        builder.Property(x => x.Id).IsRequired(true);
        builder.HasIndex(x => x.Id).IsUnique(true);
        builder.HasKey(z => z.Id);
    }
}