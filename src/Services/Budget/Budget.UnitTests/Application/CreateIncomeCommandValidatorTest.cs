using Budget.Application.Commands;
using Budget.Application.Dtos;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.IncomeAggregates;

namespace Budget.UnitTests.Application;

public class CreateIncomeCommandValidatorTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly CreateIncomeCommandValidator _validator;

    public CreateIncomeCommandValidatorTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();

        _validator = new CreateIncomeCommandValidator(_repositoryMock.Object);
    }

    [Theory]
    [InlineData(1, 100, "Test")]
    [InlineData(-1, 100, "Test")]
    [InlineData(0, 0, "Test")]
    [InlineData(0, -1, "Test")]
    [InlineData(0, 100, null)]
    [InlineData(0, 100, "")]
    [InlineData(0, 100, " ")]
    public void Validate_WhenCommandHasAnInvalidProperty_ShouldReturnFailureWithOneError(int id, decimal amount, string description)
    {
        // Arrange
        var (_, _, _, date) = GetValidIncomeConstructorParameters();

        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date
        };

        var command = new CreateIncomeCommand(dto);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
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

    [Fact]
    public void Validate_WhenIncomeWithSameDescriptionAndMonthAlreadyExists_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var command = GetValidCommand();

        _repositoryMock
            .Setup(r => r.ExistsIncomeWithDescriptionAndMonth(command.Dto.Description, command.Dto.Date.Month, command.Dto.Date.Year))
            .Returns(true);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    private static CreateIncomeCommand GetValidCommand()
    {
        var (id, amount, description, date) = GetValidIncomeConstructorParameters();

        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date
        };

        return new CreateIncomeCommand(dto);
    }

    private static (int, decimal, string, DateTime) GetValidIncomeConstructorParameters()
    {
        var id = 0;
        var amount = 100m;
        var description = "Test";
        var date = DateTime.UtcNow;

        return (id, amount, description, date);
    }
}