using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.Exceptions;

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

    public MonthlyBudgetSummaryModel GetMonthlySummary(int month, int year)
    {
        if (month < 1 || month > 12)
        {
            throw new DomainException("Month must be between 1 and 12");
        }

        if (year < 1 || year > 9999)
        {
            throw new DomainException("Year must be between 1 and 9999");
        }

        var incomes = _incomeRepository.GetIncomesByMonth(month, year);
        var expenses = _expenseRepository.GetExpensesByMonth(month, year);

        return new MonthlyBudgetSummaryModel
        {
            Year = year,
            Month = month,
            IncomeSum = incomes.Sum(x => x.Amount),
            ExpenseSum = expenses.Sum(x => x.Amount),
            Balance = incomes.Sum(x => x.Amount) - expenses.Sum(x => x.Amount),
            CategorizedExpenses = expenses.GroupBy(x => x.CategoryId)
                .Select(g => new CategorizedExpenseModel
                {
                    CategoryId = g.Key,
                    AmountSum = g.Sum(c => c.Amount)
                })
        };
    }
}