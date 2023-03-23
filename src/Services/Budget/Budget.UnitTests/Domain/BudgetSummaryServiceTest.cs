using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.Exceptions;
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

        _expenseRepositoryMock
            .Setup(x => x.GetExpensesByMonth(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(DefaultExpenses);

        _incomeRepositoryMock
            .Setup(x => x.GetIncomesByMonth(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(DefaultIncomes);
    }

    [Fact]
    public void GetMonthlySummary_ByDefault_ShouldReturnSummary()
    {
        // Arrange
        var (year, month) = DefaultYearAndMonth;
        var (incomeSum, expenseSum, balance, categorizedExpenses) = ExpectedSummaryValues;

        // Act
        var result = _service.GetMonthlySummary(year, month);

        // Assert
        Assert.Equal(year, result.Year);
        Assert.Equal(month, result.Month);
        Assert.Equal(incomeSum, result.IncomeSum);
        Assert.Equal(expenseSum, result.ExpenseSum);
        Assert.Equal(balance, result.Balance);
        Assert.Equivalent(categorizedExpenses, result.CategorizedExpenses);
    }

    [Fact]
    public void GetMonthlySummary_WhenNoData_ShouldReturnEmptySummary()
    {
        // Arrange
        var (year, month) = DefaultYearAndMonth;

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

    [Theory]
    [InlineData(0, 1)]
    [InlineData(13, 1)]
    [InlineData(1, 0)]
    [InlineData(1, 10000)]
    public void GetMonthlySummary_WhenYearOrMonthIsInvalid_ShouldThrowDomainException(int month, int year)
    {
        // Act
        var act = new Action(() => _service.GetMonthlySummary(month, year));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    private Income[] DefaultIncomes => new[]
    {
        new Income(_incomeRepository, 1_500M, "Salary", default)
    };

    private Expense[] DefaultExpenses => new[]
    {
        new Expense(_expenseRepository, 100M, "Groceries", default, ExpenseCategory.Food.Id),
        new Expense(_expenseRepository, 50M, "Coffee", default, ExpenseCategory.Food.Id),
        new Expense(_expenseRepository, 200M, "Doctor", default, ExpenseCategory.Health.Id),
        new Expense(_expenseRepository, 100M, "Rent", default, ExpenseCategory.Housing.Id),
        new Expense(_expenseRepository, 100M, "Gas", default, ExpenseCategory.Transportation.Id)
    };

    private static CategorizedExpenseModel[] ExpectedCategorizedExpenses => new[]
    {
        new CategorizedExpenseModel { CategoryId = ExpenseCategory.Food.Id, AmountSum = 150M },
        new CategorizedExpenseModel { CategoryId = ExpenseCategory.Health.Id, AmountSum = 200M },
        new CategorizedExpenseModel { CategoryId = ExpenseCategory.Housing.Id, AmountSum = 100M },
        new CategorizedExpenseModel { CategoryId = ExpenseCategory.Transportation.Id, AmountSum = 100M }
    };

    private static (decimal, decimal, decimal, CategorizedExpenseModel[]) ExpectedSummaryValues => (1_500M, 550M, 950M, ExpectedCategorizedExpenses);

    private static (int year, int month) DefaultYearAndMonth => (1, 1);
}