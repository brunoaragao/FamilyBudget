using Budget.Application.Dtos;
using Budget.Application.Requests;
using Budget.Application.Validators;

namespace Budget.UnitTests.Application;

public class UpdateExpenseRequestValidatorTest
{
    readonly UpdateExpenseRequestValidator _validator = new();
    readonly UpdateExpenseRequest _defaultRequest = new(Dto: new() { Id = 1, Amount = 100, Description = "Test", Date = default, Category = ExpenseCategoryDto.Others });

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
    [InlineData(0, 100, "Test", (ExpenseCategoryDto)1)]
    [InlineData(-1, 100, "Test", (ExpenseCategoryDto)1)]
    [InlineData(1, 0, "Test", (ExpenseCategoryDto)1)]
    [InlineData(1, -1, "Test", (ExpenseCategoryDto)1)]
    [InlineData(1, 100, null, (ExpenseCategoryDto)1)]
    [InlineData(1, 100, "", (ExpenseCategoryDto)1)]
    [InlineData(1, 100, " ", (ExpenseCategoryDto)1)]
    [InlineData(1, 100, "Test", (ExpenseCategoryDto)99)]
    public void Validate_WhenRequestHasAnInvalidValue_ShouldReturnInvalidResultWithOneError(int id, decimal amount, string description, ExpenseCategoryDto category)
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

        var request = new UpdateExpenseRequest(dto);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }
}