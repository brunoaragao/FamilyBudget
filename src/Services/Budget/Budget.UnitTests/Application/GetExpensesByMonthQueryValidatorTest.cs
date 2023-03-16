using Budget.Application.Queries;
using Budget.Application.Validators;

using FluentValidation;

namespace Budget.UnitTests.Application;

public class GetExpensesByMonthQueryValidatorTest
{
    private readonly IValidator<GetExpensesByMonthQuery> _validator;

    public GetExpensesByMonthQueryValidatorTest()
    {
        _validator = new GetExpensesByMonthQueryValidator();
    }

    [Fact]
    public void Validate_ByDefault_ShouldReturnValidResult()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(13, 0)]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    [InlineData(1, 10_000)]
    public void Validate_WhenMonthOrYearIsInvalid_ShouldReturnInvalidResult(int month, int year)
    {
        // Arrange
        var command = new GetExpensesByMonthQuery(month, year);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    private static GetExpensesByMonthQuery GetDefaultCommand()
    {
        return new GetExpensesByMonthQuery(1, 0001);
    }
}