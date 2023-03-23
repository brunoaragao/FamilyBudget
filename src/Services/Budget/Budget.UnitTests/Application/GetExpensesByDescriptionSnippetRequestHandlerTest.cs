using AutoMapper;

using Budget.Application.Queries.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;

namespace Budget.Application.Handlers;

public class GetExpensesByDescriptionSnippetRequestHandlerTest
{
    readonly Mock<IExpenseRepository> _repositoryMock = new();
    readonly GetExpensesByDescriptionSnippetRequestHandler _handler;
    readonly GetExpensesByDescriptionSnippetRequest _defaultRequest = new(DescriptionSnippet: "Test");


    public GetExpensesByDescriptionSnippetRequestHandlerTest()
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
            .Setup(r => r.GetExpensesByDescriptionSnippet(It.IsAny<string>()))
            .Returns(fakeExpenses);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithValues()
    {
        // Arrange
        var query = _defaultRequest;

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WhenDoesNotExistsExpensesWithDescriptionSnippet_ShouldReturnSuccessResultWithEmptyValues()
    {
        // Arrange
        var query = _defaultRequest;

        _repositoryMock
            .Setup(r => r.GetExpensesByDescriptionSnippet(query.DescriptionSnippet))
            .Returns(Enumerable.Empty<Expense>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}