using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.Exceptions;
using Budget.Domain.SeedWork;

namespace Budget.UnitTests.Domain;

public class ExpenseCategoryTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void FromValue_ShouldReturnEnumeration(int id)
    {
        // Act
        var category = Enumeration.FromValue<ExpenseCategory>(id);

        // Assert
        Assert.NotNull(category);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(9)]
    public void FromValue_WithInvalidId_ShouldThrowDomainException(int id)
    {
        // Act
        var act = new Action(() => _ = Enumeration.FromValue<ExpenseCategory>(id));

        // Assert
        Assert.Throws<DomainException>(act);
    }
}