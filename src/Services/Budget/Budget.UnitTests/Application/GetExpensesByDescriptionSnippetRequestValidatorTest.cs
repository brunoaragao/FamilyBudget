using Budget.Application.Requests;
using Budget.Application.Validators;

namespace Budget.UnitTests.Application;

public class GetExpensesByDescriptionSnippetRequestValidatorTest
{
    readonly GetExpensesByDescriptionSnippetRequestValidator _validator = new();
    readonly GetExpensesByDescriptionSnippetRequest _defaultRequest = new(DescriptionSnippet: "Test");

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
    public async Task Validate_WhenDescriptionSnippetIsInvalid_ShouldReturnInvalidResult(string snippet)
    {
        // Arrange
        var request = new GetExpensesByDescriptionSnippetRequest(snippet);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        Assert.False(result.IsValid);
    }
}