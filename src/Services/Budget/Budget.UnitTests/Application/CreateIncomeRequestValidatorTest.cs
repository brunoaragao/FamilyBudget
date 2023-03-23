using Budget.Application.Dtos;
using Budget.Application.Requests;
using Budget.Application.Validators;

namespace Budget.UnitTests.Application;

public class CreateIncomeRequestValidatorTest
{
    readonly CreateIncomeRequestValidator _validator = new();
    readonly CreateIncomeRequest _defaultRequest = new(Dto: new() { Id = 0, Amount = 1M, Description = "Test", Date = default });

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
    [InlineData(1, 100, "Test")]
    [InlineData(-1, 100, "Test")]
    [InlineData(0, 0, "Test")]
    [InlineData(0, -1, "Test")]
    [InlineData(0, 100, null)]
    [InlineData(0, 100, "")]
    [InlineData(0, 100, " ")]
    public void Validate_WhenRequestHasAnInvalidProperty_ShouldReturnFailureWithOneError(int id, decimal amount, string description)
    {
        // Arrange
        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = default
        };

        var request = new CreateIncomeRequest(dto);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }
}