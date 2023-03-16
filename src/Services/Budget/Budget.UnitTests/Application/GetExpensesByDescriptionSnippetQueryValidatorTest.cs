using Budget.Application.Queries;
using Budget.Application.Validators;

using FluentValidation;

namespace Budget.UnitTests.Application;

public class GetExpensesByDescriptionSnippetQueryValidatorTest
{
    private readonly IValidator<GetExpensesByDescriptionSnippetQuery> _validator;

    public GetExpensesByDescriptionSnippetQueryValidatorTest()
    {
        _validator = new GetExpensesByDescriptionSnippetQueryValidator();
    }

    [Fact]
    public async Task Validate_ByDefault_ShouldReturnValidResult()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        var result = await _validator.ValidateAsync(command);

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
        var command = new GetExpensesByDescriptionSnippetQuery(snippet);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    private static GetExpensesByDescriptionSnippetQuery GetDefaultCommand()
    {
        return new GetExpensesByDescriptionSnippetQuery("test");
    }
}