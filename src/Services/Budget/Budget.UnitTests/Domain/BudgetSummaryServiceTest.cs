using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.Services;

namespace Budget.UnitTests.Domain;

public class BudgetSummaryServiceTest
{
    private readonly Mock<IExpenseRepository> _expenseRepositoryMock;
    private readonly IExpenseRepository _expenseRepository;
    private readonly Mock<IIncomeRepository> _incomeRepositoryMock;
    private readonly IIncomeRepository _incomeRepository;
    private readonly IBudgetSummaryService _service;

    public BudgetSummaryServiceTest()
    {
        _expenseRepositoryMock = new Mock<IExpenseRepository>();
        _incomeRepositoryMock = new Mock<IIncomeRepository>();

        _expenseRepository = _expenseRepositoryMock.Object;
        _incomeRepository = _incomeRepositoryMock.Object;

        _service = new BudgetSummaryService(_incomeRepository, _expenseRepository);
    }

    [Fact]
    public void GetMonthlySummary_WhenDataExists_ShouldReturnSummary()
    {
        // Arrange
        var (year, month) = GetDefaultYearAndMonth();
        var (expectedIncomeSum, expectedExpenseSum, expectedBalance, expectedCategorizedExpenses) = GetExpectedDefaultSummaryProperties();

        _expenseRepositoryMock
            .Setup(x => x.GetExpensesByMonth(year, month))
            .Returns(GetDefaultExpenses());

        _incomeRepositoryMock
            .Setup(x => x.GetIncomesByMonth(year, month))
            .Returns(GetDefaultIncomes());

        // Act
        var result = _service.GetMonthlySummary(year, month);

        // Assert
        Assert.Equal(year, result.Year);
        Assert.Equal(month, result.Month);
        Assert.Equal(expectedIncomeSum, result.IncomeSum);
        Assert.Equal(expectedExpenseSum, result.ExpenseSum);
        Assert.Equal(expectedBalance, result.Balance);
        Assert.Equivalent(expectedCategorizedExpenses, result.CategorizedExpenses);
    }

    [Fact]
    public void GetMonthlySummary_WhenNoData_ShouldReturnEmptySummary()
    {
        // Arrange
        var (year, month) = GetDefaultYearAndMonth();

        _expenseRepositoryMock
            .Setup(x => x.GetExpensesByMonth(year, month))
            .Returns(Enumerable.Empty<Expense>());

        _incomeRepositoryMock
            .Setup(x => x.GetIncomesByMonth(year, month))
            .Returns(Enumerable.Empty<Income>());

        // Act
        var result = _service.GetMonthlySummary(year, month);

        // Assert
        Assert.Equal(year, result.Year);
        Assert.Equal(month, result.Month);
        Assert.Equal(0M, result.IncomeSum);
        Assert.Equal(0M, result.ExpenseSum);
        Assert.Equal(0M, result.Balance);
        Assert.Empty(result.CategorizedExpenses);
    }

    private IEnumerable<Expense> GetDefaultExpenses()
    {
        var (year, month) = GetDefaultYearAndMonth();

        yield return new(_expenseRepository, 100M, "Groceries", new(year, month, 1), ExpenseCategory.Food);
        yield return new(_expenseRepository, 200M, "Doctor", new(year, month, 1), ExpenseCategory.Health);
        yield return new(_expenseRepository, 100M, "Rent", new(year, month, 1), ExpenseCategory.Housing);
        yield return new(_expenseRepository, 100M, "Gas", new(year, month, 1), ExpenseCategory.Transportation);
    }

    private IEnumerable<Income> GetDefaultIncomes()
    {
        var (year, month) = GetDefaultYearAndMonth();

        yield return new(_incomeRepository, 1_500M, "Salary", new(year, month, 1));
    }

    private static (decimal incomeSum, decimal expenseSum, decimal balance, IEnumerable<CategorizedExpenseModel> expensesByCategory) GetExpectedDefaultSummaryProperties()
    {
        var incomeSum = 1_500M;
        var expenseSum = 500M;
        var balance = 1_000M;

        var expensesByCategory = new CategorizedExpenseModel[]
        {
            new() { Category = ExpenseCategory.Food, AmountSum = 100M },
            new() { Category = ExpenseCategory.Health, AmountSum = 200M },
            new() { Category = ExpenseCategory.Housing, AmountSum = 100M },
            new() { Category = ExpenseCategory.Transportation, AmountSum = 100M }
        };

        return (incomeSum, expenseSum, balance, expensesByCategory);
    }

    private static (int year, int month) GetDefaultYearAndMonth()
    {
        DateTime date = default;
        return (date.Year, date.Month);
    }
}