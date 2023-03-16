using Budget.Application.Queries;
using Budget.Application.Validators;

using FluentValidation;

namespace Budget.UnitTests.Application;

public class GetIncomesByMonthQueryValidatorTest
{
    private readonly IValidator<GetIncomesByMonthQuery> _validator;

    public GetIncomesByMonthQueryValidatorTest()
    {
        _validator = new GetIncomesByMonthQueryValidator();
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
        var command = new GetIncomesByMonthQuery(month, year);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    private static GetIncomesByMonthQuery GetDefaultCommand()
    {
        return new GetIncomesByMonthQuery(1, 0001);
    }
}