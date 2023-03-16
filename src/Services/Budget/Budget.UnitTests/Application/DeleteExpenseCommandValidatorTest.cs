using Budget.Application.Commands;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentValidation;

namespace Budget.UnitTests.Application;

public class DeleteExpenseCommandValidatorTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly IValidator<DeleteExpenseCommand> _validator;

    public DeleteExpenseCommandValidatorTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _validator = new DeleteExpenseCommandValidator(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(It.IsAny<int>()))
            .Returns(true);
    }

    [Fact]
    public async Task Validate_ByDefault_ShouldReturnValidResult()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validate_WhenIdIsInvalid_ShouldReturnInvalidResult(int id)
    {
        // Arrange
        var command = new DeleteExpenseCommand(id);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task Validate_WhenExpenseDoesNotExist_ShouldReturnInvalidResult()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(command.Id))
            .Returns(false);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    private static DeleteExpenseCommand GetDefaultCommand()
    {
        return new DeleteExpenseCommand(1);
    }
}