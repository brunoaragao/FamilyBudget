using Budget.Domain.AggregateModels.ExpenseAggregates;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Infrastructure.Data.EntityConfigurations;

public class ExpenseEntityTypeConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expense");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Description)
            .HasMaxLength(200);

        builder.Property(e => e.Date)
            .HasColumnType("date");

        builder.Property(e => e.CategoryId)
            .HasColumnName("ExpenseCategoryId");

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId);

        builder.HasIndex(e => new { e.CategoryId, e.Date });

        builder.HasIndex(e => new { e.Date });

        builder.HasIndex(e => new { e.Date, e.Description });

        builder.HasIndex(e => new { e.Description });
    }
}