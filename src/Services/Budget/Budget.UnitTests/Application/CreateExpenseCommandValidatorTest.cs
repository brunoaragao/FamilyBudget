using Budget.Application.Commands;
using Budget.Application.Dtos;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.ExpenseAggregates;

namespace Budget.UnitTests.Application;

public class CreateExpenseCommandValidatorTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly CreateExpenseCommandValidator _validator;

    public CreateExpenseCommandValidatorTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _validator = new CreateExpenseCommandValidator(_repositoryMock.Object);
    }

    [Fact]
    public void Validate_WhenCommandHasValidProperties_ShouldReturnSuccess()
    {
        // Arrange
        var command = GetValidCommand();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(1, 100, "Test", (ExpenseCategoryDto)1)]
    [InlineData(-1, 100, "Test", (ExpenseCategoryDto)1)]
    [InlineData(0, 0, "Test", (ExpenseCategoryDto)1)]
    [InlineData(0, -1, "Test", (ExpenseCategoryDto)1)]
    [InlineData(0, 100, null, (ExpenseCategoryDto)1)]
    [InlineData(0, 100, "", (ExpenseCategoryDto)1)]
    [InlineData(0, 100, " ", (ExpenseCategoryDto)1)]
    [InlineData(0, 100, "Test", (ExpenseCategoryDto)99)]
    public void Validate_WhenCommandHasAnInvalidProperty_ShouldReturnFailureWithOneError(int id, decimal amount, string description, ExpenseCategoryDto expenseCategory)
    {
        // Arrange
        var (_, _, _, date, _) = GetValidExpenseConstructorParameters();

        var dto = new ExpenseDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date,
            ExpenseCategory = expenseCategory
        };

        var command = new CreateExpenseCommand(dto);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void Validate_WhenExpenseWithSameDescriptionAndMonthAlreadyExists_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var command = GetValidCommand();

        _repositoryMock
            .Setup(r => r.ExistsExpenseWithDescriptionAndMonth(command.Dto.Description, command.Dto.Date.Month, command.Dto.Date.Year))
            .Returns(true);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    private static CreateExpenseCommand GetValidCommand()
    {
        var (id, amount, description, date, expenseCategory) = GetValidExpenseConstructorParameters();

        var dto = new ExpenseDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date,
            ExpenseCategory = expenseCategory
        };

        return new CreateExpenseCommand(dto);
    }

    private static (int, decimal, string, DateTime, ExpenseCategoryDto) GetValidExpenseConstructorParameters()
    {
        var id = 0;
        var amount = 100m;
        var description = "Test";
        var date = DateTime.UtcNow;
        var expenseCategory = ExpenseCategoryDto.Others;

        return (id, amount, description, date, expenseCategory);
    }
}