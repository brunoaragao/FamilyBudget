using Budget.Application.Requests;
using Budget.Application.Validators;

namespace Budget.UnitTests.Application;

public class DeleteIncomeRequestValidatorTest
{
    readonly DeleteIncomeRequestValidator _validator = new();
    readonly DeleteIncomeRequest _defaultRequest = new(Id: 1);

    [Fact]
    public async Task Validate_ByDefault_ShouldReturnValidResult()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validate_WhenIdIsInvalid_ShouldReturnInvalidResult(int id)
    {
        // Arrange
        var request = new DeleteIncomeRequest(id);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        Assert.False(result.IsValid);
    }
}