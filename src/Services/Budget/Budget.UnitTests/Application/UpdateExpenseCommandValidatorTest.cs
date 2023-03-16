using Budget.Application.Commands;
using Budget.Application.Dtos;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.ExpenseAggregates;

namespace Budget.UnitTests.Application;

public class UpdateExpenseCommandValidatorTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly UpdateExpenseCommandValidator _validator;

    public UpdateExpenseCommandValidatorTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _validator = new UpdateExpenseCommandValidator(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(It.IsAny<int>()))
            .Returns(true);
    }

    [Fact]
    public void Validate_WhenCommandHasValidValues_ShouldReturnValidResultWithNoErrors()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0, 100, "Test", (ExpenseCategoryDto)1)]
    [InlineData(-1, 100, "Test", (ExpenseCategoryDto)1)]
    [InlineData(1, 0, "Test", (ExpenseCategoryDto)1)]
    [InlineData(1, -1, "Test", (ExpenseCategoryDto)1)]
    [InlineData(1, 100, null, (ExpenseCategoryDto)1)]
    [InlineData(1, 100, "", (ExpenseCategoryDto)1)]
    [InlineData(1, 100, " ", (ExpenseCategoryDto)1)]
    [InlineData(1, 100, "Test", (ExpenseCategoryDto)99)]
    public void Validate_WhenCommandHasAnInvalidValue_ShouldReturnInvalidResultWithOneError(int id, decimal amount, string description, ExpenseCategoryDto expenseCategory)
    {
        // Arrange
        var (_, _, _, date, _) = GetValidExpenseDtoValuesToUpdate();

        var dto = new ExpenseDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date,
            ExpenseCategory = expenseCategory
        };

        var command = new UpdateExpenseCommand(dto);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void Validate_WhenExpenseDoesNotExist_ShouldReturnInvalidResultWithOneError()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(command.Dto.Id))
            .Returns(false);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void Validate_WhenExistsAnotherExpenseWithDescriptionAndMonth_ShouldReturnInvalidResultWithOneError()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsAnotherExpenseWithDescriptionAndMonth(command.Dto.Id, command.Dto.Description, command.Dto.Date.Month, command.Dto.Date.Year))
            .Returns(true);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    private static UpdateExpenseCommand GetDefaultCommand()
    {
        var (id, amount, description, date, expenseCategory) = GetValidExpenseDtoValuesToUpdate();

        var dto = new ExpenseDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date,
            ExpenseCategory = expenseCategory
        };

        return new UpdateExpenseCommand(dto);
    }

    private static (int, decimal, string, DateTime, ExpenseCategoryDto) GetValidExpenseDtoValuesToUpdate()
    {
        return (1, 100, "Test", default(DateTime), ExpenseCategoryDto.Others);
    }
}