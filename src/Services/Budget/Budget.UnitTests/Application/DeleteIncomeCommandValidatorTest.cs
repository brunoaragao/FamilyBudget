using Budget.Application.Commands;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentValidation;

namespace Budget.UnitTests.Application;

public class DeleteIncomeCommandValidatorTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly IValidator<DeleteIncomeCommand> _validator;

    public DeleteIncomeCommandValidatorTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();
        _validator = new DeleteIncomeCommandValidator(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(It.IsAny<int>()))
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
        var command = new DeleteIncomeCommand(id);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task Validate_WhenIncomeDoesNotExist_ShouldReturnInvalidResult()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(command.Id))
            .Returns(false);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    private static DeleteIncomeCommand GetDefaultCommand()
    {
        return new DeleteIncomeCommand(1);
    }
}