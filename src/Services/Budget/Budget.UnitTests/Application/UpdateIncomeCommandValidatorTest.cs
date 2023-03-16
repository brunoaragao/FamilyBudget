using Budget.Application.Commands;
using Budget.Application.Dtos;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.IncomeAggregates;

namespace Budget.UnitTests.Application;

public class UpdateIncomeCommandValidatorTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly UpdateIncomeCommandValidator _validator;

    public UpdateIncomeCommandValidatorTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();
        _validator = new UpdateIncomeCommandValidator(_repositoryMock.Object);

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(It.IsAny<int>()))
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
    [InlineData(0, 100, "Test")]
    [InlineData(-1, 100, "Test")]
    [InlineData(1, 0, "Test")]
    [InlineData(1, -1, "Test")]
    [InlineData(1, 100, null)]
    [InlineData(1, 100, "")]
    [InlineData(1, 100, " ")]
    public void Validate_WhenCommandHasAnInvalidValue_ShouldReturnInvalidResultWithOneError(int id, decimal amount, string description)
    {
        // Arrange
        var (_, _, _, date) = GetValidIncomeDtoValuesToUpdate();

        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date
        };

        var command = new UpdateIncomeCommand(dto);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void Validate_WhenIncomeDoesNotExist_ShouldReturnInvalidResultWithOneError()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(command.Dto.Id))
            .Returns(false);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void Validate_WhenExistsAnotherIncomeWithDescriptionAndMonth_ShouldReturnInvalidResultWithOneError()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsAnotherIncomeWithDescriptionAndMonth(command.Dto.Id, command.Dto.Description, command.Dto.Date.Month, command.Dto.Date.Year))
            .Returns(true);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    private static UpdateIncomeCommand GetDefaultCommand()
    {
        var (id, amount, description, date) = GetValidIncomeDtoValuesToUpdate();

        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date
        };

        return new UpdateIncomeCommand(dto);
    }

    private static (int, decimal, string, DateTime) GetValidIncomeDtoValuesToUpdate()
    {
        return (1, 100, "Test", default(DateTime));
    }
}