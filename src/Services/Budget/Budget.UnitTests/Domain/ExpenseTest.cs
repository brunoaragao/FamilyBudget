using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.Exceptions;

namespace Budget.UnitTests.Domain;

public class ExpenseTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly IExpenseRepository _repository;

    public ExpenseTest()
    {
        _repositoryMock = new();
        _repository = _repositoryMock.Object;
    }

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        var (amount, description, date, category) = GetValidConstructorParameters();

        // Act
        var expense = new Expense(_repository, amount, description, date, category);

        // Assert
        Assert.Equal(amount, expense.Amount);
        Assert.Equal(description, expense.Description);
        Assert.Equal(date, expense.Date);
        Assert.Equal(category, expense.ExpenseCategory);
    }

    [Fact]
    public void Constructor_WhenCategoryIsNotProvided_ShouldSetCategoryToOthers()
    {
        // Arrange
        var (amount, description, date, _) = GetValidConstructorParameters();

        // Act
        var expense = new Expense(_repository, amount, description, date);

        // Assert
        Assert.Equal(ExpenseCategory.Others, expense.ExpenseCategory);
    }

    [Fact]
    public void Constructor_WhenExpenseWithSameDescriptionAndMonthAlreadyExists_ShouldThrowDomainException()
    {
        // Arrange
        var (amount, description, date, category) = GetValidConstructorParameters();

        _repositoryMock
            .Setup(r => r.ExistsExpenseWithDescriptionAndMonth(description, date.Month, date.Year))
            .Returns(true);

        // Act
        var act = new Action(() => _ = new Expense(_repository, amount, description, date, category));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0, "Test")]
    [InlineData(-1, "Test")]
    [InlineData(100, null)]
    [InlineData(100, "")]
    [InlineData(100, " ")]
    public void Constructor_WhenParametersAreInvalid_ShouldThrowDomainException(decimal amount, string description)
    {
        // Arrange
        var (_, _, date, category) = GetValidConstructorParameters();

        // Act
        var act = new Action(() => _ = new Expense(_repository, amount, description, date, category));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Update_ShouldSetProperties()
    {
        // Arrange
        var expense = GetValidExpense();
        var (amount, description, date, category) = GetValidUpdateParameters();

        // Act
        expense.Update(_repository, amount, description, date, category);

        // Assert
        Assert.Equal(amount, expense.Amount);
        Assert.Equal(description, expense.Description);
        Assert.Equal(date, expense.Date);
        Assert.Equal(category, expense.ExpenseCategory);
    }

    [Fact]
    public void Update_WhenExpenseWithSameDescriptionAndMonthAlreadyExists_ShouldThrowDomainException()
    {
        // Arrange
        var expense = GetValidExpense();
        var (amount, description, date, category) = GetValidUpdateParameters();

        _repositoryMock
            .Setup(r => r.ExistsAnotherExpenseWithDescriptionAndMonth(expense.Id, description, date.Month, date.Year))
            .Returns(true);

        // Act
        var act = new Action(() => expense.Update(_repository, amount, description, date, category));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0, "Test")]
    [InlineData(-1, "Test")]
    [InlineData(100, null)]
    [InlineData(100, "")]
    [InlineData(100, " ")]
    public void Update_WhenParametersAreInvalid_ShouldThrowDomainException(decimal amount, string description)
    {
        // Arrange
        var expense = GetValidExpense();
        var (_, _, date, category) = GetValidUpdateParameters();

        // Act
        var act = new Action(() => expense.Update(_repository, amount, description, date, category));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Update_WhenParametersAreInvalid_ShouldNotChangeProperties()
    {
        // Arrange
        var (initialAmount, initialDescription, initialDate, initialCategory) = GetValidConstructorParameters();

        var (amount, description) = GetInvalidUpdateParameters();
        var (_, _, date, category) = GetValidUpdateParameters();

        var expense = new Expense(_repository, initialAmount, initialDescription, initialDate, initialCategory);

        // Act
        var act = new Action(() => expense.Update(_repository, amount, description, date, category));

        // Assert
        Assert.Throws<DomainException>(act);
        Assert.Equal(initialAmount, expense.Amount);
        Assert.Equal(initialDescription, expense.Description);
        Assert.Equal(initialDate, expense.Date);
        Assert.Equal(initialCategory, expense.ExpenseCategory);
    }

    private Expense GetValidExpense()
    {
        var (amount, description, date, category) = GetValidConstructorParameters();
        return new(_repository, amount, description, date, category);
    }

    private static (decimal, string, DateTime, ExpenseCategory) GetValidConstructorParameters()
    {
        var amount = 100M;
        var description = "Test";
        var date = DateTime.UtcNow;
        var category = ExpenseCategory.Others;

        return (amount, description, date, category);
    }

    private static (decimal, string, DateTime, ExpenseCategory) GetValidUpdateParameters()
    {
        var amount = 200M;
        var description = "Test 2";
        var date = DateTime.UtcNow.AddMonths(1);
        var category = ExpenseCategory.Food;

        return (amount, description, date, category);
    }

    private static (decimal, string) GetInvalidUpdateParameters()
    {
        var amount = 0M;
        var description = string.Empty;

        return (amount, description);
    }
}