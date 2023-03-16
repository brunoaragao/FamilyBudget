using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.AggregateModels.IncomeAggregates;

namespace Budget.Domain.Services;

public class BudgetSummaryService : IBudgetSummaryService
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IExpenseRepository _expenseRepository;

    public BudgetSummaryService(IIncomeRepository incomeRepository, IExpenseRepository expenseRepository)
    {
        _incomeRepository = incomeRepository;
        _expenseRepository = expenseRepository;
    }

    // MonthlySummary
    public MonthlyBudgetSummaryModel GetMonthlySummary(int month, int year)
    {
        var incomes = _incomeRepository.GetIncomesByMonth(month, year);
        var expenses = _expenseRepository.GetExpensesByMonth(month, year);

        return new MonthlyBudgetSummaryModel
        {
            Year = year,
            Month = month,
            IncomeSum = incomes.Sum(x => x.Amount),
            ExpenseSum = expenses.Sum(x => x.Amount),
            Balance = incomes.Sum(x => x.Amount) - expenses.Sum(x => x.Amount),
            CategorizedExpenses = expenses.GroupBy(x => x.ExpenseCategory)
                .Select(g => new CategorizedExpenseModel
                {
                    Category = g.Key,
                    AmountSum = g.Sum(c => c.Amount)
                })
        };
    }
}