using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.AggregateModels.IncomeAggregates;

using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Data;

public class BudgetContext : DbContext
{
    public const string DEFAULT_SCHEMA = "Budget";

    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Income> Incomes => Set<Income>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetContext).Assembly);
    }
}