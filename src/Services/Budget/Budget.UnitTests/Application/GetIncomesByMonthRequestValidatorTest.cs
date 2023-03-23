using Budget.Application.Requests;
using Budget.Application.Validators;

namespace Budget.UnitTests.Application;

public class GetIncomesByMonthRequestValidatorTest
{
    readonly GetIncomesByMonthRequestValidator _validator = new();

    readonly GetIncomesByMonthRequest _defaultRequest = new(Month: 1, Year: 1);

    [Fact]
    public void Validate_ByDefault_ShouldReturnValidResult()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = _validator.Validate(request);

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
        var request = new GetIncomesByMonthRequest(month, year);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
    }
}