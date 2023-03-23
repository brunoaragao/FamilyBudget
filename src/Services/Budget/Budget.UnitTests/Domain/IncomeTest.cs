using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.Exceptions;

namespace Budget.UnitTests.Domain;

public class IncomeTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly IIncomeRepository _repository;

    public IncomeTest()
    {
        _repositoryMock = new();
        _repository = _repositoryMock.Object;
    }

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        var (amount, description, date) = GetValidConstructorParameters();

        // Act
        var income = new Income(_repository, amount, description, date);

        // Assert
        Assert.Equal(amount, income.Amount);
        Assert.Equal(description, income.Description);
        Assert.Equal(date, income.Date);
    }

    [Fact]
    public void Constructor_WhenIncomeWithSameDescriptionAndMonthAlreadyExists_ShouldThrowDomainException()
    {
        // Arrange
        var (amount, description, date) = GetValidConstructorParameters();

        _repositoryMock
            .Setup(r => r.ExistsIncomeWithDescriptionAndMonth(description, date.Month, date.Year))
            .Returns(true);

        // Act
        var act = new Action(() => _ = new Income(_repository, amount, description, date));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0, "Test")]
    [InlineData(-1, "Test")]
    [InlineData(100, null)]
    [InlineData(100, "")]
    public void Constructor_WhenParametersAreInvalid_ShouldThrowDomainException(decimal amount, string description)
    {
        // Arrange
        var (_, _, date) = GetValidConstructorParameters();

        // Act
        var act = new Action(() => _ = new Income(_repository, amount, description, date));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Update_ShouldSetProperties()
    {
        // Arrange
        var income = GetValidIncome();
        var (amount, description, date) = GetValidUpdateParameters();

        // Act
        income.Update(_repository, amount, description, date);

        // Assert
        Assert.Equal(amount, income.Amount);
        Assert.Equal(description, income.Description);
        Assert.Equal(date, income.Date);
    }

    [Fact]
    public void Update_WhenIncomeWithSameDescriptionAndMonthAlreadyExists_ShouldThrowDomainException()
    {
        // Arrange
        var income = GetValidIncome();
        var (amount, description, date) = GetValidUpdateParameters();

        _repositoryMock
            .Setup(r => r.ExistsAnotherIncomeWithDescriptionAndMonth(income.Id, description, date.Month, date.Year))
            .Returns(true);

        // Act
        var act = new Action(() => income.Update(_repository, amount, description, date));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0, "Test")]
    [InlineData(-1, "Test")]
    [InlineData(100, null)]
    [InlineData(100, "")]
    public void Update_WhenParametersAreInvalid_ShouldThrowDomainException(decimal amount, string description)
    {
        // Arrange
        var income = GetValidIncome();
        var (_, _, date) = GetValidUpdateParameters();

        // Act
        var act = new Action(() => income.Update(_repository, amount, description, date));

        // Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Update_WhenParametersAreInvalid_ShouldNotChangeProperties()
    {
        // Arrange
        var (initialAmount, initialDescription, initialDate) = GetValidConstructorParameters();
        var income = new Income(_repository, initialAmount, initialDescription, initialDate);

        var (_, _, date) = GetValidUpdateParameters();
        var (amount, description) = GetInvalidUpdateParameters();


        // Act
        var act = new Action(() => income.Update(_repository, amount, description, date));

        // Assert
        Assert.Throws<DomainException>(act);
        Assert.Equal(initialAmount, income.Amount);
        Assert.Equal(initialDescription, income.Description);
        Assert.Equal(initialDate, income.Date);
    }

    private static (decimal, string, DateTime) GetValidConstructorParameters()
    {
        var amount = 100M;
        var description = "Test";
        var date = DateTime.UtcNow;

        return (amount, description, date);
    }

    private static (decimal, string, DateTime) GetValidUpdateParameters()
    {
        var amount = 200M;
        var description = "Test 2";
        var date = DateTime.UtcNow.AddMonths(1);

        return (amount, description, date);
    }

    private static (decimal, string) GetInvalidUpdateParameters()
    {
        var amount = 0M;
        var description = string.Empty;

        return (amount, description);
    }

    private Income GetValidIncome()
    {
        var (amount, description, date) = GetValidConstructorParameters();
        return new Income(_repository, amount, description, date);
    }
}