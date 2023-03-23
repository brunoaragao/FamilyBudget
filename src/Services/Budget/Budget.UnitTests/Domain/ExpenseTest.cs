using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.Exceptions;

namespace Budget.UnitTests.Domain;

public class ExpenseTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly IExpenseRepository _repository;

    public ExpenseTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _repository = _repositoryMock.Object;
    }

    private Expense DefaultExpense => new(_repository, 1, "Test", default, 1);
    private static (decimal, string, DateTime, int) ValidConstructorParameters => (1, "Test", default, 1);
    private static (decimal, string, DateTime, int) ValidUpdateParameters => (2, "Test 2", default, 2);

    private static (decimal, string, int) InvalidUpdateParameters => (0, string.Empty, 0);


    [Fact]
    public void Constructor_ByDefault_ShouldSetProperties()
    {
        // Arrange
        var (amount, description, date, categoryId) = ValidConstructorParameters;

        // Act
        var expense = new Expense(_repository, amount, description, date, categoryId);

        // Assert
        Assert.Equal(amount, expense.Amount);
        Assert.Equal(description, expense.Description);
        Assert.Equal(date, expense.Date);
        Assert.Equal(categoryId, expense.CategoryId);
    }

    [Fact]
    public void Constructor_WhenCategoryIdIsNotProvided_ShouldSetCategoryIdToOthersId()
    {
        // Arrange
        var (amount, description, date, _) = ValidConstructorParameters;

        // Act
        var expense = new Expense(_repository, amount, description, date);

        // Assert
        Assert.Equal(ExpenseCategory.Others.Id, expense.CategoryId);
    }

    [Fact]
    public void Constructor_WhenExpenseWithSameDescriptionAndMonthAlreadyExists_ShouldThrowDomainException()
    {
        // Arrange
        var (amount, description, date, categoryId) = ValidConstructorParameters;

        _repositoryMock
            .Setup(r => r.ExistsExpenseWithDescriptionAndMonth(description, date.Month, date.Year))
            .Returns(true);

        // Act
        var act = new Action(() => _ = new Expense(_repository, amount, description, date, categoryId));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0, "Test", 1)]
    [InlineData(-1, "Test", 1)]
    [InlineData(1, null, 1)]
    [InlineData(1, "", 1)]
    [InlineData(1, " ", 1)]
    [InlineData(1, "Test", 0)]
    [InlineData(1, "Test", 9)]
    public void Constructor_WhenParametersAreInvalid_ShouldThrowDomainException(decimal amount, string description, int categoryId)
    {
        // Arrange
        var (_, _, date, _) = ValidConstructorParameters;

        // Act
        var act = new Action(() => _ = new Expense(_repository, amount, description, date, categoryId));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Update_ByDefault_ShouldSetProperties()
    {
        // Arrange
        var expense = DefaultExpense;
        var (amount, description, date, categoryId) = ValidUpdateParameters;

        // Act
        expense.Update(_repository, amount, description, date, categoryId);

        // Assert
        Assert.Equal(amount, expense.Amount);
        Assert.Equal(description, expense.Description);
        Assert.Equal(date, expense.Date);
        Assert.Equal(categoryId, expense.CategoryId);
    }

    [Fact]
    public void Update_WhenExpenseWithSameDescriptionAndMonthAlreadyExists_ShouldThrowDomainException()
    {
        // Arrange
        var expense = DefaultExpense;
        var (amount, description, date, category) = ValidUpdateParameters;

        _repositoryMock
            .Setup(r => r.ExistsAnotherExpenseWithDescriptionAndMonth(expense.Id, description, date.Month, date.Year))
            .Returns(true);

        // Act
        var act = new Action(() => expense.Update(_repository, amount, description, date, category));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0, "Test", 1)]
    [InlineData(-1, "Test", 1)]
    [InlineData(1, null, 1)]
    [InlineData(1, "", 1)]
    [InlineData(1, " ", 1)]
    [InlineData(1, "Test", 0)]
    [InlineData(1, "Test", 9)]
    public void Update_WhenParametersAreInvalid_ShouldThrowDomainException(decimal amount, string description, int categoryId)
    {
        // Arrange
        var expense = DefaultExpense;
        var (_, _, date, _) = ValidUpdateParameters;

        // Act
        var act = new Action(() => expense.Update(_repository, amount, description, date, categoryId));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Update_WhenParametersAreInvalid_ShouldNotChangeProperties()
    {
        // Arrange
        var (initialAmount, initialDescription, initialDate, initialCategoryId) = ValidConstructorParameters;

        var (newAmount, newDescription, newCategoryId) = InvalidUpdateParameters;
        var (_, _, newDate, _) = ValidUpdateParameters;

        var expense = new Expense(_repository, initialAmount, initialDescription, initialDate, initialCategoryId);

        // Act
        var act = new Action(() => expense.Update(_repository, newAmount, newDescription, newDate, newCategoryId));

        // Assert
        Assert.Throws<DomainException>(act);
        Assert.Equal(initialAmount, expense.Amount);
        Assert.Equal(initialDescription, expense.Description);
        Assert.Equal(initialDate, expense.Date);
        Assert.Equal(initialCategoryId, expense.CategoryId);
    }
}