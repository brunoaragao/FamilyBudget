using Budget.Domain.AggregateModels.ExpenseAggregates;

namespace Budget.Domain.Services;

public class CategorizedExpenseModel
{
    public required ExpenseCategory Category { get; set; }
    public required decimal AmountSum { get; set; }
}