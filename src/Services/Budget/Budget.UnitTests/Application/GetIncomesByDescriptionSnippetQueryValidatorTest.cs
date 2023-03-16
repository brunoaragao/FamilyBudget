using Budget.Application.Queries;
using Budget.Application.Validators;

using FluentValidation;

namespace Budget.UnitTests.Application;

public class GetIncomesByDescriptionSnippetQueryValidatorTest
{
    private readonly IValidator<GetIncomesByDescriptionSnippetQuery> _validator;

    public GetIncomesByDescriptionSnippetQueryValidatorTest()
    {
        _validator = new GetIncomesByDescriptionSnippetQueryValidator();
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
        var command = new GetIncomesByDescriptionSnippetQuery(snippet);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    private static GetIncomesByDescriptionSnippetQuery GetDefaultCommand()
    {
        return new GetIncomesByDescriptionSnippetQuery("test");
    }
}