namespace Budget.Application.Dtos;

public class MonthlyBudgetSummaryDto
{
    public required int Month { get; set; }
    public required int Year { get; set; }
    public required decimal IncomeSum { get; set; }
    public required decimal ExpenseSum { get; set; }
    public required decimal Balance { get; set; }
    public required IEnumerable<CategorizedExpenseDto> CategorizedExpenses { get; set; }
}