namespace Budget.Domain.Services;

public class MonthlyBudgetSummaryModel
{
    public required int Month { get; set; }
    public required int Year { get; set; }
    public required decimal IncomeSum { get; set; }
    public required decimal ExpenseSum { get; set; }
    public required decimal Balance { get; set; }
    public required IEnumerable<CategorizedExpenseModel> CategorizedExpenses { get; set; }
}