using Budget.Domain.AggregateModels.IncomeAggregates;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Data.EntityConfigurations;

public class IncomeEntityTypeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.ToTable("Income");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Description)
            .HasMaxLength(200);

        builder.Property(i => i.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.Date);

        builder.HasIndex(i => new { i.Date });

        builder.HasIndex(i => new { i.Description });
    }
}