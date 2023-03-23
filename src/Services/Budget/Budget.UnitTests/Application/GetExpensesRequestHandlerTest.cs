using AutoMapper;

using Budget.Application.Queries.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;

namespace Budget.UnitTests.Application;

public class GetExpensesRequestHandlerTest
{
    readonly Mock<IExpenseRepository> _repositoryMock = new();
    readonly GetExpensesRequestHandler _handler;
    readonly GetExpensesRequest _defaultRequest = new();

    public GetExpensesRequestHandlerTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        var repository = _repositoryMock.Object;

        Expense[] fakeExpenses =
        {
            new(repository, 1M, "Test", default, ExpenseCategory.Others.Id),
            new(repository, 1M, "Test", default, ExpenseCategory.Others.Id)
        };

        _handler = new(repository, mapper);

        _repositoryMock
            .Setup(r => r.GetExpenses())
            .Returns(fakeExpenses);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithExpenses()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WhenDoesNotExistsExpenses_ShouldReturnSuccessResultWithEmptyExpenses()
    {
        // Arrange
        var query = _defaultRequest;

        _repositoryMock
            .Setup(r => r.GetExpenses())
            .Returns(Enumerable.Empty<Expense>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}