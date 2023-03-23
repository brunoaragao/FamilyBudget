using Budget.Application.Requests;
using Budget.Application.Validators;

namespace Budget.UnitTests.Application;

public class GetIncomesByDescriptionSnippetRequestValidatorTest
{
    readonly GetIncomesByDescriptionSnippetRequestValidator _validator = new();
    readonly GetIncomesByDescriptionSnippetRequest _defaultRequest = new(DescriptionSnippet: "test");

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
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Validate_WhenDescriptionIsInvalid_ShouldReturnInvalidResult(string snippet)
    {
        // Arrange
        var request = new GetIncomesByDescriptionSnippetRequest(snippet);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        Assert.False(result.IsValid);
    }
}