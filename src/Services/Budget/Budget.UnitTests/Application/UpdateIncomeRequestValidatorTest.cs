using Budget.Application.Dtos;
using Budget.Application.Requests;
using Budget.Application.Validators;

namespace Budget.UnitTests.Application;

public class UpdateIncomeRequestValidatorTest
{
    readonly UpdateIncomeRequestValidator _validator = new();
    readonly UpdateIncomeRequest _defaultRequest = new(Dto: new() { Id = 1, Amount = 100, Description = "Test", Date = default });

    [Fact]
    public void Validate_ByDefault_ShouldReturnValidResult()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = _validator.Validate(request);

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
    public void Validate_WhenRequestHasAnInvalidValue_ShouldReturnInvalidResultWithOneError(int id, decimal amount, string description)
    {
        // Arrange
        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = default
        };

        var request = new UpdateIncomeRequest(dto);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }
}