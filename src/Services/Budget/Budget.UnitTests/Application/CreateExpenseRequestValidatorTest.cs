using Budget.Application.Dtos;
using Budget.Application.Requests;
using Budget.Application.Validators;

namespace Budget.UnitTests.Application;

public class CreateExpenseRequestValidatorTest
{
    readonly CreateExpenseRequestValidator _validator = new();
    readonly CreateExpenseRequest _defaultRequest = new(Dto: new() { Id = 0, Amount = 1M, Description = "Test", Date = default, Category = ExpenseCategoryDto.Others });

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
    [InlineData(1, 100, "Test", (ExpenseCategoryDto)1)]
    [InlineData(-1, 100, "Test", (ExpenseCategoryDto)1)]
    [InlineData(0, 0, "Test", (ExpenseCategoryDto)1)]
    [InlineData(0, -1, "Test", (ExpenseCategoryDto)1)]
    [InlineData(0, 100, null, (ExpenseCategoryDto)1)]
    [InlineData(0, 100, "", (ExpenseCategoryDto)1)]
    [InlineData(0, 100, " ", (ExpenseCategoryDto)1)]
    [InlineData(0, 100, "Test", (ExpenseCategoryDto)99)]
    public void Validate_WhenRequestHasAnInvalidProperty_ShouldReturnFailureWithOneError(int id, decimal amount, string description, ExpenseCategoryDto category)
    {
        // Arrange
        var dto = new ExpenseDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = default,
            Category = category
        };

        var request = new CreateExpenseRequest(dto);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }
}